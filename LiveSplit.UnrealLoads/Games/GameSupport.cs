using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace LiveSplit.DXLoads.Games
{
	public enum IdentificationResult
	{
		Success,
		Failure,
		Undecisive
	}

	public enum TimerAction
	{
		DoNothing,
		Start,
		Reset,
		Split,
		PauseGameTime,
		UnpauseGameTime
	}

	abstract class GameSupport
	{
		public abstract HashSet<string> GameNames { get; }

		public abstract HashSet<string> ProcessNames { get; }

		public virtual HashSet<string> Maps { get; } = new HashSet<string>();

		public virtual Type LoadMapDetourT => typeof(LoadMapDetour);

		public virtual Type SaveGameDetourT => typeof(SaveGameDetour);

		public Detour GetLoadMapHook(Process game, IntPtr setMapPtr, IntPtr statusPtr)
		{
			if (LoadMapDetourT == null)
				throw new Exception("No LoadMapDetour type defined");

			var originalPtr = Detour.FindExportedFunc(LoadMapDetourT, game);
			if (originalPtr != IntPtr.Zero)
				return (Detour)Activator.CreateInstance(LoadMapDetourT, setMapPtr, statusPtr);
			else
				return null;
		}

		public SaveGameDetour GetSaveGameHook(Process game, IntPtr statusPtr)
		{
			if (SaveGameDetourT == null)
				throw new Exception("No SaveGameDetour type defined");

			var originalPtr = Detour.FindExportedFunc(SaveGameDetourT, game);
			if (originalPtr != IntPtr.Zero)
				return (SaveGameDetour)Activator.CreateInstance(SaveGameDetourT, statusPtr);
			else
				return null;
		}

		public string[] GetHookModules()
		{
			var list = new List<string>();
			var loadmap = Detour.GetModule(LoadMapDetourT);
			var savegame = Detour.GetModule(SaveGameDetourT);
			if (!string.IsNullOrEmpty(loadmap))
				list.Add(loadmap);
			if (!string.IsNullOrEmpty(savegame))
				list.Add(savegame);
			return list.ToArray();
		}

		public virtual IdentificationResult IdentifyProcess(Process process) => IdentificationResult.Success;

		public virtual TimerAction[] OnUpdate(Process game, MemoryWatcherList watchers) => null;

		public virtual TimerAction[] OnMapLoad(MemoryWatcherList watchers) => null;

		public virtual bool? IsLoading(MemoryWatcherList watchers) => null;

		public virtual TimerAction[] OnAttach(Process game) => null;

		public virtual TimerAction[] OnDetach(Process game) => null;
	}

}
