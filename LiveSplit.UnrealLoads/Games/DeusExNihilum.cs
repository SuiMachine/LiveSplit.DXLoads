using LiveSplit.ComponentUtil;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;

namespace LiveSplit.DXLoads.Games
{
	class DeusExNihilum : GameSupport
	{
		public override HashSet<string> GameNames => new HashSet<string>
		{
			"Deus Ex Nihilum"
		};

		public override HashSet<string> ProcessNames => new HashSet<string>
		{
			"fgrhk"
		};

		public override Type SaveGameDetourT => typeof(SaveGameDetour_DeusEx);

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

		class SaveGameDetour_DeusEx : SaveGameDetour
		{
			public new static string Symbol => "?SaveCurrentLevel@DDeusExGameEngine@@QAEXH_N@Z";
			public new static string Module => "DeusEx.dll";

			public SaveGameDetour_DeusEx(IntPtr statusAddr) : base(statusAddr)
			{
				_statusPtr = statusAddr;
			}

			public override byte[] GetBytes()
			{
				var status = _statusPtr.ToBytes().ToHex();
				var none = Status.None.ToBytes().ToHex();
				var saving = Status.Saving.ToBytes().ToHex();


				var str = string.Join("\n",
					"55",                            //push ebp
					"8B EC",                         //mov ebp,esp
					"83 EC 08",                      //sub esp,8
					"89 55 F8",                      //mov dword ptr ss:[ebp-8],edx
					"89 4D FC",                      //mov dword ptr ss:[ebp-4],ecx
					"C7 05 " + status + saving,      //mov dword ptr ds:[<g_status>],2
					"0F B6 45 0C",                   //movzx eax,byte ptr ss:[ebp+C]
					"50",                            //push eax
					"8B 4D 08",                      //mov ecx,dword ptr ss:[ebp+8]
					"51",                            //push ecx
					"8B 4D FC",                      //mov ecx,dword ptr ss:[ebp-4]
					"#FF FF FF FF FF",               //call dword ptr ds:[118378C]
					"C7 05 " + status + none,		 //mov dword ptr ds:[<g_status>],0
					"8B E5",                         //mov esp,ebp
					"5D",                            //pop ebp
					"C2 08 00"                       //ret 8
				);

				int[] offsets;
				var bytes = Utils.ParseBytes(str, out offsets);
				_originalFuncCallOffset = offsets[0];

				return bytes.ToArray();
			}
		}
	}
}
