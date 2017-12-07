using LiveSplit.ComponentUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace LiveSplit.DXLoads.Games
{
	class Nihilum : GameSupport
	{
		public override HashSet<string> GameNames => new HashSet<string>
		{
			"Nihilum"
		};

		public override HashSet<string> ProcessNames => new HashSet<string>
		{
			"fgrhk"
		};

		public override Type SaveGameDetourT => typeof(SaveGameDetour);

		StringWatcher _map;

		public override HashSet<string> Maps => new HashSet<string>
		{
			"60_hongkong_forichi",
			"60_hongkong_greaselpit",
			"60_hongkong_mpshelipad",
			"60_hongkong_mpshq",
			"60_hongkong_streets",
			"61_hongkong_factory",
			"61_hongkong_forichi",
			"61_hongkong_mpshelipad",
			"61_hongkong_streets",
			"61_hongkong_tianbaohotel",
			"62_berlin_airbornelab",
			"62_berlin_airport",
			"62_berlin_streets",
			"62_berlin_xvacomplex",
			"62_berlin_xvainterior",
			"63_nyc_cutterresidence",
			"63_nyc_queensstreets",
			"63_nyc_storage",
			"64_woodfibre_stafford",
			"64_woodfibre_tunnels",
			"65_woodfibre_beachfront",
			"66_whitehouse_exterior",
			"66_whitehouse_illuminati",
			"66_whitehouse_streets",
			"67_dynamene_exterior",
			"67_dynamene_innersection",
			"67_dynamene_missileunit",
			"67_dynamene_outersection",
			"68_ending_1",
			"68_ending_2",
			"68_ending_3",
			"68_ending_4"
		};

		public override TimerAction[] OnMapLoad(MemoryWatcherList watchers)
		{
			var status = (MemoryWatcher<int>)watchers["status"];
			_map = (StringWatcher)watchers["map"];



			if (status.Current == (int)Status.LoadingMap)
			{
				if (_map.Current.ToLower() == "59_intro")
					return new TimerAction[] { TimerAction.Reset };
				else if (_map.Current.ToLower() == "60_hongkong_mpshelipad")
					return new TimerAction[] { TimerAction.Start };
			}


			return null;
		}
	}
}
