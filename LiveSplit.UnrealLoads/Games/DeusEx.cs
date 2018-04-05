﻿using LiveSplit.ComponentUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace LiveSplit.DXLoads.Games
{
	class DeusEx : GameSupport
	{
		public override HashSet<string> GameNames => new HashSet<string>
		{
			"DeusEx",
			"Deus Ex"
		};

		public override HashSet<string> ProcessNames => new HashSet<string>
		{
			"deusex"
		};

		public override Type SaveGameDetourT => typeof(SaveGameDetour);

		StringWatcher _map;

		public override HashSet<string> Maps => new HashSet<string>
		{
			"01_nyc_unatcoisland",
			"01_nyc_unatcohq",
			"02_nyc_batterypark",
			"02_nyc_street",
			"02_nyc_warehouse",
			"02_nyc_bar",
			"02_nyc_freeclinic",
			"02_nyc_hotel",
			"02_nyc_smug",
			"02_nyc_underground",
			"03_nyc_unatcoisland",
			"03_nyc_unatcohq",
			"03_nyc_batterypark",
			"03_nyc_brooklynbridgestation",
			"03_nyc_molepeople",
			"03_nyc_airfieldhelibase",
			"03_nyc_airfield",
			"03_nyc_hangar",
			"04_nyc_unatcoisland",
			"04_nyc_unatcohq",
			"04_nyc_street",
			"04_nyc_hotel",
			"04_nyc_bar",
			"04_nyc_batterypark",
			"04_nyc_nsfhq",
			"04_nyc_smug",
			"04_nyc_underground",
			"05_nyc_unatcomj12lab",
			"05_nyc_unatcohq",
			"05_nyc_unatcoisland",
			"06_hongkong_helibase",
			"06_hongkong_wanchai_market",
			"06_hongkong_versalife",
			"06_hongkong_mj12lab",
			"06_hongkong_storage",
			"06_hongkong_wanchai_canal",
			"06_hongkong_tongbase",
			"06_hongkong_wanchai_garage",
			"06_hongkong_wanchai_street",
			"06_hongkong_wanchai_underworld",
			"08_nyc_street",
			"08_nyc_bar",
			"08_nyc_freeclinic",
			"08_nyc_hotel",
			"08_nyc_smug",
			"08_nyc_underground",
			"09_nyc_dockyard",
			"09_nyc_shipfan",
			"09_nyc_ship",
			"09_nyc_shipbelow",
			"09_nyc_graveyard",
			"10_paris_catacombs",
			"10_paris_catacombs_tunnels",
			"10_paris_metro",
			"10_paris_club",
			"10_paris_metro",
			"10_paris_chateau",
			"11_paris_cathedral",
			"11_paris_underground",
			"11_paris_everett",
			"12_vandenberg_cmd",
			"12_vandenberg_gas",
			"14_vandenberg_sub",
			"12_vandenberg_computer",
			"12_vandenberg_tunnels",
			"14_oceanlab_lab",
			"14_oceanlab_uc",
			"14_oceanlab_silo",
			"15_area51_bunker",
			"15_area51_entrance",
			"15_area51_final",
			"15_area51_page",
			"99_endgame1",
			"99_endgame2",
			"99_endgame3",
			"99_endgame4"
		};

		public override TimerAction[] OnMapLoad(MemoryWatcherList watchers)
		{
			var status = (MemoryWatcher<int>)watchers["status"];
			_map = (StringWatcher)watchers["map"];



			if (status.Current == (int)Status.LoadingMap)
			{
				if (_map.Current.ToLower() == "00_intro")
					return new TimerAction[] { TimerAction.Reset };
				else if (_map.Current.ToLower() == "01_nyc_unatcoisland")
					return new TimerAction[] { TimerAction.Start };
			}

			return null;
		}
	}
}
