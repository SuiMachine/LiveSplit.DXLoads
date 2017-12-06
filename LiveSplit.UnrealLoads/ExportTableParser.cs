using LiveSplit.ComponentUtil;
using PE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LiveSplit.DXLoads
{
	//didn't test 64-bit
	public class ExportTableParser
	{
		public ReadOnlyDictionary<string, IntPtr> Exports { get; private set; }

		Process _process;
		ProcessModuleWow64Safe _module;
		IMAGE_EXPORT_DIRECTORY _ied;

		public ExportTableParser(Process process, string module = null)
		{
			_process = process;
			_module = !string.IsNullOrEmpty(module)
				? process.ModulesWow64Safe().FirstOrDefault(m => m.ModuleName.ToLower() == module.ToLower())
				: process.MainModuleWow64Safe();

			if (_module == null)
				throw new ArgumentException("Module not found.", nameof(module));
		}

		public void Parse()
		{
			var idh = _process.ReadValue<IMAGE_DOS_HEADER>(_module.BaseAddress);
			if (!idh.isValid)
				throw new Exception("Invalid header.");
			var exportTable = _process.Is64Bit()
				? _process.ReadValue<IMAGE_NT_HEADERS64>(_module.BaseAddress + idh.e_lfanew).OptionalHeader.ExportTable
				: _process.ReadValue<IMAGE_NT_HEADERS32>(_module.BaseAddress + idh.e_lfanew).OptionalHeader.ExportTable;
			_ied = _process.ReadValue<IMAGE_EXPORT_DIRECTORY>(_module.BaseAddress + (int)exportTable.VirtualAddress);
			Exports = new ReadOnlyDictionary<string, IntPtr>(GetExports());
		}

		public IntPtr GetAddressFromOrdinal(int ordinal)
		{
			var ptr = _module.BaseAddress + (int)_ied.AddressOfFunctions + ordinal * sizeof(int);
			return _module.BaseAddress + _process.ReadValue<int>(ptr);
		}

		int GetNameOrdinal(int index)
		{
			var ptr = _module.BaseAddress + (int)_ied.AddressOfNameOrdinals + index * sizeof(short);
			return _process.ReadValue<short>(ptr);
		}

		Dictionary<string, IntPtr> GetExports()
		{
			var namesPtr = _module.BaseAddress + (int)_ied.AddressOfNames;
			var dict = new Dictionary<string, IntPtr>();

			for (int i = 0; i < _ied.NumberOfNames; i++)
			{
				var namePtr = _module.BaseAddress + _process.ReadValue<int>(namesPtr + i * sizeof(int));

				var sb = new StringBuilder();
				while (true)
				{
					var character = (char)_process.ReadBytes(namePtr + sb.Length, 1)[0];
					if (character == '\0')
						break;
					sb.Append(character);
				}

				dict.Add(sb.ToString(), GetAddressFromOrdinal(GetNameOrdinal(i)));
			}

			return dict;
		}
	}
}
