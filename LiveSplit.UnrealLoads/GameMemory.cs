﻿using LiveSplit.ComponentUtil;
using LiveSplit.DXLoads.Games;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.DXLoads
{
	enum Status
	{
		None,
		LoadingMap,
		Saving
	}

	class GameMemory
	{
		public const int SLEEP_TIME = 15;

		public static readonly GameSupport[] SupportedGames = new GameSupport[]
		{
			new DeusEx(),
			new Nihilum(),
			new Revision()
		};

		public static readonly string[] SupportedProcessesNames =
			SupportedGames.SelectMany(g => g.ProcessNames
				.Select(pName => pName.ToLower()))
				.ToArray();

		public event EventHandler OnReset;
		public event EventHandler OnStart;
		public event EventHandler OnSplit;
		public event EventHandler OnLoadStarted;
		public event EventHandler OnLoadEnded;
		public event MapChangeEventHandler OnMapChange;
		public delegate void MapChangeEventHandler(object sender, string map);

		public GameSupport Game { get; private set; }

		Task _thread;
		CancellationTokenSource _cancelSource;
		SynchronizationContext _uiThread;
		HashSet<int> _ignorePIDs;
		int _lastPID;

		MemoryWatcherList _watchers;
		MemoryWatcher<int> _status;
		StringWatcher _map;

		SetMapFunction _setMapFunc;
		Detour _loadMapHook;
		Detour _saveGameHook;
		IntPtr _statusPtr;
		IntPtr _mapPtr;

		readonly int MAP_SIZE = Encoding.Unicode.GetMaxByteCount(260); // MAX_PATH == 260

		public GameMemory()
		{
			_ignorePIDs = new HashSet<int>();
			_watchers = new MemoryWatcherList();
		}

		public void StartMonitoring()
		{
			if (_thread != null && _thread.Status == TaskStatus.Running)
				throw new InvalidOperationException();

			if (!(SynchronizationContext.Current is WindowsFormsSynchronizationContext))
				throw new InvalidOperationException("SynchronizationContext.Current is not a UI thread.");

			_uiThread = SynchronizationContext.Current;
			_cancelSource = new CancellationTokenSource();
			_thread = Task.Factory.StartNew(MemoryReadThread);
		}

		public void Stop()
		{
			if (_cancelSource == null || _thread == null || _thread.Status != TaskStatus.Running)
				return;

			_cancelSource.Cancel();
			_thread.Wait();
		}

		void MemoryReadThread()
		{
			Process game = null;

			while (!_cancelSource.IsCancellationRequested)
			{
				try
				{
					Trace.WriteLine("[NoLoads] Waiting for a game process...");

					while ((game = GetGameProcess()) == null)
					{
						Thread.Sleep(250);
						if (_cancelSource.IsCancellationRequested)
							return;
					}

					Trace.WriteLine($"[NoLoads] Attached to {game.ProcessName}.exe ({Game.GetType().Name})");
					_uiThread.Post(d => OnLoadEnded?.Invoke(this, EventArgs.Empty), null); //unpause at launch

					uint frameCounter = 0;
					bool isLoading;
					bool prevIsLoading = false;
					var map = string.Empty;
					var prevMap = string.Empty;

					DoTimerAction(Game.OnAttach(game));

					while (!game.HasExited)
					{
						_watchers.UpdateAll(game);
						DoTimerAction(Game.OnUpdate(game, _watchers));

						var gameSupportIsLoading = Game.IsLoading(_watchers);
						if (!gameSupportIsLoading.HasValue)
							isLoading = _status.Current != (int)Status.None;
						else
							isLoading = gameSupportIsLoading.Value;

						Debug.WriteLineIf(_status.Changed, string.Format("[NoLoads] Status changed from {1} to {2} - {0}", frameCounter, (Status)_status.Old, (Status)_status.Current));

						if (_map.Changed)
						{
							map = Path.GetFileNameWithoutExtension(_map.Current).ToLower();
							_uiThread.Post(d => OnMapChange?.Invoke(this, map), null);
							Debug.WriteLine(string.Format("[NoLoads] Map is changing from \"{0}\" to \"{1}\" - {2}", prevMap, map, frameCounter));
						}

						if (_status.Changed && _status.Current == (int)Status.LoadingMap)
						{
							DoTimerAction(Game.OnMapLoad(_watchers));
						}

						if (isLoading != prevIsLoading)
						{
							if (isLoading)
							{
								_uiThread.Post(d => OnLoadStarted?.Invoke(this, EventArgs.Empty), null);
								Trace.WriteLine(string.Format("[NoLoads] Load start - {0}", frameCounter));
							}
							else
							{
								_uiThread.Post(d => OnLoadEnded?.Invoke(this, EventArgs.Empty), null);
								Trace.WriteLine(string.Format("[NoLoads] Load end - {0}", frameCounter));
							}
						}

						prevIsLoading = isLoading;
						prevMap = map;
						frameCounter++;

						Thread.Sleep(SLEEP_TIME);

						if (_cancelSource.IsCancellationRequested)
							break;
					}

					//pause on crash/exit
					_uiThread.Post(d => OnLoadStarted?.Invoke(this, EventArgs.Empty), null);
					DoTimerAction(Game.OnDetach(game));
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.ToString());
					Thread.Sleep(1000);
				}
			}

			Unpatch(game);
		}

		void DoTimerAction(TimerAction action)
		{
			EventHandler evnt = null;
			switch (action)
			{
				case TimerAction.DoNothing:
					break;
				case TimerAction.Reset:
					evnt = OnReset;
					break;
				case TimerAction.Start:
					evnt = OnStart;
					break;
				case TimerAction.Split:
					evnt = OnSplit;
					break;
				case TimerAction.PauseGameTime:
					evnt = OnLoadStarted;
					break;
				case TimerAction.UnpauseGameTime:
					evnt = OnLoadEnded;
					break;
			}

			_uiThread.Post(d => evnt?.Invoke(this, EventArgs.Empty), null);
		}

		void DoTimerAction(IEnumerable<TimerAction> actions)
		{
			if (actions == null)
				return;

			foreach (var action in actions)
				DoTimerAction(action);
		}

		Process GetGameProcess()
		{
			Process game = null;

			var processes = SupportedProcessesNames.SelectMany(n => Process.GetProcessesByName(n))
				.OrderByDescending(p => p.StartTime);

			foreach (var p in processes)
			{
				if (p.HasExited || _ignorePIDs.Contains(p.Id))
					continue;

				var ignoreProcess = true;
				foreach (var gameSupport in SupportedGames)
				{
					if (!gameSupport.ProcessNames.Contains(p.ProcessName.ToLower()))
						continue;

					var modules = p.ModulesWow64Safe();
					if (!gameSupport.GetHookModules().All(hook_m =>
								modules.Any(m => hook_m.ToLower() == m.ModuleName.ToLower())))
					{
						ignoreProcess = false;
						continue;
					}

					switch (gameSupport.IdentifyProcess(p))
					{
						case IdentificationResult.Success:
							game = p;
							Game = gameSupport;
							break;
						case IdentificationResult.Undecisive:
							ignoreProcess = false; // don't ignore if at least one game is unsure
							break;
					}

					if (game != null)
						break;
				}

				if (game != null)
					break;

				if (ignoreProcess)
					_ignorePIDs.Add(p.Id);
			}

			if (game == null)
				return null;

			if (_lastPID != game.Id)
			{
				_watchers.Clear();

				if (!Patch(game))
					return null;

				_status = new MemoryWatcher<int>(_statusPtr) { Name = "status" };
				_map = new StringWatcher(_mapPtr, ReadStringType.UTF16, MAP_SIZE) { Name = "map" };
				_watchers.AddRange(new MemoryWatcher[] { _status, _map });
			}

			_lastPID = game.Id;
			return game;
		}

		bool Patch(Process game)
		{
			game.Suspend();
			try
			{
				_statusPtr = game.AllocateMemory(sizeof(int));
				_mapPtr = game.AllocateMemory(MAP_SIZE);
				_setMapFunc = new SetMapFunction(_mapPtr);
				_setMapFunc.Inject(game);

				_loadMapHook = Game.GetLoadMapHook(game, _setMapFunc.InjectedFuncPtr, _statusPtr);
				_saveGameHook = Game.GetSaveGameHook(game, _statusPtr);

				if (_loadMapHook == null)
					throw new Exception("Couldn't find the LoadMap function.");

				if (_saveGameHook == null)
					throw new Exception("Couldn't find the SaveGame function.");

				_saveGameHook.Install(game);
				_loadMapHook.Install(game);

				Debug.WriteLine($"[NoLoads] Status: {_statusPtr.ToString("X")} Map: {_mapPtr.ToString("X")} ");
				Debug.WriteLine($"[NoLoads] FakeSaveGame: {_saveGameHook.InjectedFuncPtr.ToString("X")} FakeLoadMap: {_loadMapHook.InjectedFuncPtr.ToString("X")}");
				Debug.WriteLine($"[NoLoads] SaveGame: {_saveGameHook.DetouredFuncPtr.ToString("X")} LoadMap: {_loadMapHook.DetouredFuncPtr.ToString("X")}");
				Debug.WriteLine("[NoLoads] Hooks installed");
			}
			catch
			{
				FreeMemory(game);
				return false;
			}
			finally
			{
				game.Resume();
			}

			return true;
		}

		bool Unpatch(Process game)
		{
			if (game == null || game.HasExited)
				return false;

			game.Suspend();
			try
			{
				_saveGameHook.Uninstall(game);
				_loadMapHook.Uninstall(game);
			}
			catch
			{
				Debug.WriteLine("[NoLoads] Unpatching failed.");
				return false;
			}
			finally
			{
				game.Resume();
				FreeMemory(game);
			}

			return true;
		}

		void FreeMemory(Process game)
		{
			if (game == null || game.HasExited)
				return;

			game.FreeMemory(_statusPtr);
			game.FreeMemory(_mapPtr);
			_setMapFunc?.FreeMemory(game);
			_saveGameHook?.FreeMemory(game);
			_loadMapHook?.FreeMemory(game);
		}
	}
}
