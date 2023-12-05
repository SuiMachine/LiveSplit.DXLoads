﻿using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.DXLoads
{
	public class DXLoadsComponent : LogicComponent
	{
		public override string ComponentName => "DXLoads";

		public DXLoadsSettings Settings { get; set; }

		TimerModel _timer;
		GameMemory _gameMemory;
		LiveSplitState _state;
		HashSet<string> _splitHistory;

		public DXLoadsComponent(LiveSplitState state)
		{
			bool debug = false;
#if DEBUG
			debug = true;
#endif
			Trace.WriteLine("[NoLoads] Using LiveSplit.DXLoads component version " + Assembly.GetExecutingAssembly().GetName().Version + " " + ((debug) ? "Debug" : "Release") + " build");

			_state = state;
			_timer = new TimerModel { CurrentState = state };
			_splitHistory = new HashSet<string>();

			Settings = new DXLoadsSettings(_state);

			_state.OnStart += _state_OnStart;

			_gameMemory = new GameMemory();
			_gameMemory.OnReset += gameMemory_OnReset;
			_gameMemory.OnStart += gameMemory_OnStart;
			_gameMemory.OnSplit += _gameMemory_OnSplit;
			_gameMemory.OnLoadStarted += gameMemory_OnLoadStarted;
			_gameMemory.OnLoadEnded += gameMemory_OnLoadEnded;
			_gameMemory.OnMapChange += _gameMemory_OnMapChange;

			_gameMemory.StartMonitoring();
		}

		void _state_OnStart(object sender, EventArgs e)
		{
			_timer.InitializeGameTime();
			_splitHistory.Clear();
		}

		void _gameMemory_OnSplit(object sender, EventArgs e)
		{
			_timer.Split();
		}

		string oldMap = "";

		void _gameMemory_OnMapChange(object sender, string map)
		{
			map = map.ToLower();
			if (Settings.AutoSplitOnMapChange)
			{
				var enabled = false;
				if (Settings.Maps.Count == 0)
					enabled = true;
				else
					Settings.Maps.TryGetValue(map, out enabled);

				if (enabled && map != oldMap)
				{
					_timer.Split();
					_splitHistory.Add(map);
				}
			}

			if (Settings.AutoSplitExceptOnRepeatMaps && !_splitHistory.Contains(map))
			{
				var enabled = false;
				if (Settings.Maps.Count == 0)
					enabled = true;
				else
					Settings.Maps.TryGetValue(map, out enabled);

				if (enabled && map != oldMap)
				{
					_timer.Split();
					_splitHistory.Add(map);
				}
			}

			if (Settings.NoAutoSplit)
			{
				return;
			}
			oldMap = map;

#if DEBUG
			if (Settings.DbgShowMap)
				MessageBox.Show(_state.Form, "Map name: \"" + map + "\"", "LiveSplit.DXLoads",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
		}

		public override void Dispose()
		{
			_gameMemory?.Stop();
			_state.OnStart -= _state_OnStart;
		}

		void gameMemory_OnReset(object sender, EventArgs e)
		{
			if (Settings.AutoReset)
			{
				_timer.Reset();
			}
		}

		void gameMemory_OnStart(object sender, EventArgs e)
		{
			if (_state.CurrentPhase == TimerPhase.NotRunning && Settings.AutoStart)
			{
				_timer.Start();
			}
		}

		void gameMemory_OnLoadStarted(object sender, EventArgs e)
		{
			_state.IsGameTimePaused = true;
		}

		void gameMemory_OnLoadEnded(object sender, EventArgs e)
		{
			_state.IsGameTimePaused = false;
		}

		public override XmlNode GetSettings(XmlDocument document) => Settings.GetSettings(document);

		public override Control GetSettingsControl(LayoutMode mode) => Settings;

		public override void SetSettings(XmlNode settings) => Settings.SetSettings(settings);

		public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
	}
}
