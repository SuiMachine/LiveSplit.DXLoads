using LiveSplit.ComponentUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace LiveSplit.DXLoads.Games
{
	class TNM : GameSupport
	{
		public override HashSet<string> GameNames => new HashSet<string>
		{
			"The Nameless Mod",
		};

		public override HashSet<string> ProcessNames => new HashSet<string>
		{
			"tnm"
		};

		public override Type SaveGameDetourT => typeof(SaveGameDetour);

		StringWatcher _map;

		public override HashSet<string> Maps => new HashSet<string>
		{
			"20_despot",
			"20_dxo",
			"20_fccorporate",
			"20_fccorpsewers",
			"20_fcdowntown",
			"20_fcslums",
			"20_goatcity",
			"20_goattemplae",
			"20_llamatemple",
			"20_nsc",
			"20_osc01",
			"20_osc02",
			"20_osc03",
			"20_osc04",
			"20_partyzone",
			"20_pdxhq01",
			"20_pdxhq02",
			"20_phasapartment",
			"20_solsbar",
			"20_voodooshop",
			"20_wcfloor1",
			"20_wcfloor2",
			"20_wcfloor3",
			"20_wcfloor4",
			"20_wcsublevel",
			"20_weaponshop",
			"21_dxi_caverns",
			"21_dxi_frontend",
			"21_dxi_hq",
			"21_dxi_mcr",
			"21_dxi_ruins",
			"22_atc",
			"22_dxediting",
			"22_dxo",
			"22_fccorporate",
			"22_fccorpsewers",
			"22_fcdowntown",
			"22_fcslums",
			"22_fcslumsewers",
			"22_goatcity",
			"22_goattemplae",
			"22_llamatemple",
			"22_pdxhq01",
			"22_pdxhq02",
			"22_solsbar",
			"22_voodooshop",
			"22_wcfloor1",
			"22_wcfloor4",
			"22_wcsublevel",
			"22_weaponshop",
			"23_abiexterior",
			"23_abiinterior",
			"23_abilabs",
			"23_abiruins",
			"24_spacestation01",
			"24_spacestation02",
			"24_spacestation03",
			"24_spacestation04",
			"25_tnmtraining",
			"platform_01",
			"platform_02",
			"tnmdenouement",
			"tnmendgame01",
			"tnmendgame02",
			"tnmendgame03",
		};

		public override TimerAction[] OnMapLoad(MemoryWatcherList watchers)
		{
			var status = (MemoryWatcher<int>)watchers["status"];
			_map = (StringWatcher)watchers["map"];



			if(status.Current == (int)Status.LoadingMap)
			{
				if(_map.Current.ToLower() == "tnmintro")
					return new TimerAction[] { TimerAction.Reset };
				else if(_map.Current.ToLower() == "20_phasapartment")
					return new TimerAction[] { TimerAction.Start };
			}

			return null;
		}
	}
}
