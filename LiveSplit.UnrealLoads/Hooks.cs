using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LiveSplit.DXLoads
{
	public abstract class Detour
	{
		static Dictionary<int, Dictionary<string, ExportTableParser>> ExportsCache = new Dictionary<int, Dictionary<string, ExportTableParser>>();

		public static string Symbol { get; }
		public static string Module { get; }

		public IntPtr Pointer { get; protected set; }
		public IntPtr InjectedFuncPtr { get; protected set; }
		public IntPtr DetouredFuncPtr { get; protected set; }

		protected int _originalFuncCallOffset = -1;
		protected bool _usesTrampoline;
		protected int _overwrittenBytes = 5;

		public abstract byte[] GetBytes();

		public virtual void Install(Process process, IntPtr funcToDetour)
		{
			Pointer = funcToDetour;
			if (process != null && Pointer != IntPtr.Zero)
				DetouredFuncPtr = process.ReadJMP(Pointer);
			_usesTrampoline = DetouredFuncPtr == IntPtr.Zero;

			Debug.WriteLine($"[NoLoads] Hooking using {(_usesTrampoline ? "trampoline" : "thunk")}...");

			var bytes = GetBytes();

			if (InjectedFuncPtr == IntPtr.Zero)
				InjectedFuncPtr = process.AllocateMemory(bytes.Length);

			if (_usesTrampoline)
			{
				DetouredFuncPtr = process.WriteDetour(Pointer, _overwrittenBytes, InjectedFuncPtr);
			}
			else
			{
				// in case of thunks just replace them
				process.WriteJumpInstruction(Pointer, InjectedFuncPtr);
			}

			process.WriteBytes(InjectedFuncPtr, bytes);
			if (_originalFuncCallOffset >= 0)
				process.WriteCallInstruction(InjectedFuncPtr + _originalFuncCallOffset, DetouredFuncPtr);
		}

		public void Install(Process process) => Install(process, FindExportedFunc(GetType(), process));

		public virtual void Uninstall(Process process)
		{
			if (InjectedFuncPtr == IntPtr.Zero)
				throw new InvalidOperationException("Not installed.");

			if (_usesTrampoline)
			{
				if (DetouredFuncPtr == IntPtr.Zero)
					throw new InvalidOperationException("Not installed.");
				process.CopyMemory(DetouredFuncPtr, Pointer, _overwrittenBytes);
			}
			else
			{
				process.WriteJumpInstruction(Pointer, DetouredFuncPtr);
			}
		}

		public virtual void FreeMemory(Process process)
		{
			if (process == null || process.HasExited)
				return;

			process.FreeMemory(InjectedFuncPtr);
			InjectedFuncPtr = IntPtr.Zero;
			if (_usesTrampoline)
			{
				process.FreeMemory(DetouredFuncPtr);
				DetouredFuncPtr = IntPtr.Zero;
			}
		}

		public static ExportTableParser GetExportTableParser(Process process, string module)
		{
			module = module?.ToLower() ?? string.Empty;

			Dictionary<string, ExportTableParser> tables;
			if (!ExportsCache.TryGetValue(process.Id, out tables))
			{
				tables = new Dictionary<string, ExportTableParser>();
				ExportsCache.Add(process.Id, tables);
			}

			ExportTableParser parser;
			if (!tables.TryGetValue(module, out parser))
			{
				parser = new ExportTableParser(process, module);
				parser.Parse();
				tables.Add(module, parser);
			}

			return parser;
		}

		public static IntPtr FindExportedFunc(Type type, Process process)
		{
			var symbol = GetSymbol(type);
			var module = GetModule(type);

			if (symbol == null)
				throw new Exception("No symbol defined");

			var exportParser = GetExportTableParser(process, module);
			IntPtr ptr;
			if (exportParser.Exports.TryGetValue(symbol, out ptr))
				return ptr;
			else
				return IntPtr.Zero;
		}

		public static string GetSymbol(Type type)
		{
			var prop = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			return (string)prop.First(p => p.Name == nameof(Symbol)).GetValue(null);
		}

		public static string GetModule(Type type)
		{
			var prop = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			return (string)prop.First(p => p.Name == nameof(Module)).GetValue(null);
		}
	}

	public class LoadMapDetour : Detour
	{
		public new static string Symbol => "?LoadMap@UGameEngine@@UAEPAVULevel@@ABVFURL@@PAVUPendingLevel@@PBV?$TMap@VFString@@V1@@@AAVFString@@@Z";
		public new static string Module => "engine.dll";

		protected int _setMapCallOffset;
		protected IntPtr _setMapPtr;
		protected IntPtr _statusPtr;

		public override void Install(Process process, IntPtr funcToDetour)
		{
			base.Install(process, funcToDetour);
			process.WriteCallInstruction(InjectedFuncPtr + _setMapCallOffset, _setMapPtr);
		}

		public LoadMapDetour(IntPtr setMapAddr, IntPtr statusAddr)
		{
			_setMapPtr = setMapAddr;
			_statusPtr = statusAddr;
		}

		public override byte[] GetBytes()
		{
			var status = _statusPtr.ToBytes().ToHex();
			var none = Status.None.ToBytes().ToHex();
			var loadingMap = Status.LoadingMap.ToBytes().ToHex();

			var str = string.Join("\n",
				"55",                           // push ebp
				"8B EC",                        // mov ebp,esp
				"83 EC 10",                     // sub esp,10
				"89 55 F0",                     // mov dword ptr ds:[ebp-10],edx
				"89 4D F8",                     // mov dword ptr ds:[ebp-8],ecx
				"8B 45 08",                     // mov eax,dword ptr ds:[ebp+8]
				"8B 48 1C",                     // mov ecx,dword ptr ds:[eax+1C]
				"89 4D FC",                     // mov dword ptr ds:[ebp-4],ecx
				"8B 55 FC",                     // mov edx,dword ptr ds:[ebp-4]
				"52",                           // push edx
				"#FF FF FF FF FF",              // call set_map
				"83 C4 04",                     // add esp,4
				"C7 05 " + status + loadingMap, // mov dword ptr ds:[<?g_status@@3HA>],1
				"8B 45 14",                     // mov eax,dword ptr ds:[ebp+14]
				"50",                           // push eax
				"8B 4D 10",                     // mov ecx,dword ptr ds:[ebp+10]
				"51",                           // push ecx
				"8B 55 0C",                     // mov edx,dword ptr ds:[ebp+C]
				"52",                           // push edx
				"8B 45 08",                     // mov eax,dword ptr ds:[ebp+8]
				"50",                           // push eax
				"8B 4D F8",                     // mov ecx,dword ptr ds:[ebp-8]
				"#FF FF FF FF FF",              // call dword ptr ds:[B3780]
				"89 45 F4",                     // mov dword ptr ds:[ebp-C],eax
				"C7 05 " + status + none,       // mov dword ptr ds:[<?g_status@@3HA>],0
				"8B 45 F4",                     // mov eax,dword ptr ds:[ebp-C]
				"8B E5",                        // mov esp,ebp
				"5D",                           // pop ebp
				"C2 10 00"                      // ret 10
			);

			int[] offsets;
			var bytes = Utils.ParseBytes(str, out offsets);
			_setMapCallOffset = offsets[0];
			_originalFuncCallOffset = offsets[1];

			return bytes.ToArray();
		}
	}

	public class LoadMapDetourHX : Detour
	{
		public new static string Symbol => "?LoadMap@UHXGameEngine@@UAEPAVULevel@@ABVFURL@@PAVUPendingLevel@@PBV?$TMap@VFString@@V1@@@AAVFString@@@Z";
		public new static string Module => "hx.dll";

		protected int _setMapCallOffset;
		protected IntPtr _setMapPtr;
		protected IntPtr _statusPtr;

		public override void Install(Process process, IntPtr funcToDetour)
		{
			base.Install(process, funcToDetour);
			process.WriteCallInstruction(InjectedFuncPtr + _setMapCallOffset, _setMapPtr);
		}

		public LoadMapDetourHX(IntPtr setMapAddr, IntPtr statusAddr)
		{
			_setMapPtr = setMapAddr;
			_statusPtr = statusAddr;
		}

		public override byte[] GetBytes()
		{
			var status = _statusPtr.ToBytes().ToHex();
			var none = Status.None.ToBytes().ToHex();
			var loadingMap = Status.LoadingMap.ToBytes().ToHex();

			var str = string.Join("\n",
				"55",                           // push ebp
				"8B EC",                        // mov ebp,esp
				"83 EC 10",                     // sub esp,10
				"89 55 F0",                     // mov dword ptr ds:[ebp-10],edx
				"89 4D F8",                     // mov dword ptr ds:[ebp-8],ecx
				"8B 45 08",                     // mov eax,dword ptr ds:[ebp+8]
				"8B 48 1C",                     // mov ecx,dword ptr ds:[eax+1C]
				"89 4D FC",                     // mov dword ptr ds:[ebp-4],ecx
				"8B 55 FC",                     // mov edx,dword ptr ds:[ebp-4]
				"52",                           // push edx
				"#FF FF FF FF FF",              // call set_map
				"83 C4 04",                     // add esp,4
				"C7 05 " + status + loadingMap, // mov dword ptr ds:[<?g_status@@3HA>],1
				"8B 45 14",                     // mov eax,dword ptr ds:[ebp+14]
				"50",                           // push eax
				"8B 4D 10",                     // mov ecx,dword ptr ds:[ebp+10]
				"51",                           // push ecx
				"8B 55 0C",                     // mov edx,dword ptr ds:[ebp+C]
				"52",                           // push edx
				"8B 45 08",                     // mov eax,dword ptr ds:[ebp+8]
				"50",                           // push eax
				"8B 4D F8",                     // mov ecx,dword ptr ds:[ebp-8]
				"#FF FF FF FF FF",              // call dword ptr ds:[B3780]
				"89 45 F4",                     // mov dword ptr ds:[ebp-C],eax
				"C7 05 " + status + none,       // mov dword ptr ds:[<?g_status@@3HA>],0
				"8B 45 F4",                     // mov eax,dword ptr ds:[ebp-C]
				"8B E5",                        // mov esp,ebp
				"5D",                           // pop ebp
				"C2 10 00"                      // ret 10
			);

			int[] offsets;
			var bytes = Utils.ParseBytes(str, out offsets);
			_setMapCallOffset = offsets[0];
			_originalFuncCallOffset = offsets[1];

			return bytes.ToArray();
		}
	}

	public class SaveGameDetour : Detour
	{
		public new static string Symbol => "?SaveCurrentLevel@DDeusExGameEngine@@QAEXH_N@Z";
		public new static string Module => "DeusEx.dll";

		protected IntPtr _statusPtr;

		public SaveGameDetour(IntPtr statusAddr)
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
				"C7 05 " + status + none,        //mov dword ptr ds:[<g_status>],0
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

	public class SetMapFunction
	{
		public byte[] Bytes => _bytes.ToArray();
		public IntPtr InjectedFuncPtr { get; private set; }

		List<byte> _bytes;

		public IntPtr Inject(Process process)
		{
			InjectedFuncPtr = process.AllocateMemory(Bytes.Length);
			process.WriteBytes(InjectedFuncPtr, Bytes);
			return InjectedFuncPtr;
		}

		public void FreeMemory(Process process)
		{
			if (process == null || process.HasExited)
				return;

			process.FreeMemory(InjectedFuncPtr);
			InjectedFuncPtr = IntPtr.Zero;
		}

		public SetMapFunction(IntPtr mapAddr)
		{
			var map = mapAddr.ToBytes().ToHex();

			var str = string.Join("\n",
				"55",                   // push ebp
				"8B EC",                // mov ebp,esp
				"83 EC 08",             // sub esp,8
				"C7 45 FC 0 0 0 0",     // mov dword ptr ds:[ebp-4],0
				"EB 09",                // jmp hooks.A1018
				"8B 45 FC",             // mov eax,dword ptr ds:[ebp-4]
				"83 C0 01",             // add eax,1
				"89 45 FC",             // mov dword ptr ds:[ebp-4],eax
				"81 7D FC 4 1 0 0",     // cmp dword ptr ds:[ebp-4],104
				"7D 27",                // jge hooks.A1048
				"8B 4D FC",             // mov ecx,dword ptr ds:[ebp-4]
				"8B 55 FC",             // mov edx,dword ptr ds:[ebp-4]
				"8B 45 08",             // mov eax,dword ptr ds:[ebp+8]
				"66 8B 14 50",          // mov dx,word ptr ds:[eax+edx*2]
				"66 89 14 4D " + map,   // mov word ptr ds:[ecx*2+<?g_map@@3PA_WA>
				"8B 45 FC",             // mov eax,dword ptr ds:[ebp-4]
				"8B 4D 08",             // mov ecx,dword ptr ds:[ebp+8]
				"0F B7 14 41",          // movzx edx,word ptr ds:[ecx+eax*2]
				"85 D2",                // test edx,edx
				"75 02",                // jne hooks.A1046
				"EB 02",                // jmp hooks.A1048
				"EB C7",                // jmp hooks.A100F
				"B8 2 0 0 0",           // mov eax,2
				"69 C8 3 1 0 0",        // imul ecx,eax,103
				"89 4D F8",             // mov dword ptr ds:[ebp-8],ecx
				"81 7D F8 8 2 0 0",     // cmp dword ptr ds:[ebp-8],208
				"73 02",                // jae hooks.A1061
				"EB 05",                // jmp hooks.A1066
				"E8 44 02 00 00",       // call hooks.A12AA
				"33 D2",                // xor edx,edx
				"8B 45 F8",             // mov eax,dword ptr ds:[ebp-8]
				"66 89 90" + map,       // mov word ptr ds:[eax+<?g_map@@3PA_WA>],
				"8B E5",                // mov esp,ebp
				"5D",                   // pop ebp
				"C3"                    // ret
			);

			_bytes = Utils.ParseBytes(str);
		}
	}
}
