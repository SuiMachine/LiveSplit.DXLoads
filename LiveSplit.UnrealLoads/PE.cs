using System.Runtime.InteropServices;

namespace PE
{
	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_DOS_HEADER
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public char[] e_magic;    // Magic number
		public ushort e_cblp;     // Bytes on last page of file
		public ushort e_cp;       // Pages in file
		public ushort e_crlc;     // Relocations
		public ushort e_cparhdr;  // Size of header in paragraphs
		public ushort e_minalloc; // Minimum extra paragraphs needed
		public ushort e_maxalloc; // Maximum extra paragraphs needed
		public ushort e_ss;       // Initial (relative) SS value
		public ushort e_sp;       // Initial SP value
		public ushort e_csum;     // Checksum
		public ushort e_ip;       // Initial IP value
		public ushort e_cs;       // Initial (relative) CS value
		public ushort e_lfarlc;   // File address of relocation table
		public ushort e_ovno;     // Overlay number
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public ushort[] e_res1;   // Reserved words
		public ushort e_oemid;    // OEM identifier (for e_oeminfo)
		public ushort e_oeminfo;  // OEM information; e_oemid specific
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public ushort[] e_res2;   // Reserved words
		public int e_lfanew;      // File address of new exe header
		public bool isValid => new string(e_magic) == "MZ";
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_EXPORT_DIRECTORY
	{
		public uint Characteristics;
		public uint TimeDateStamp;
		public ushort MajorVersion;
		public ushort MinorVersion;
		public uint Name;
		public uint Base;
		public uint NumberOfFunctions;
		public uint NumberOfNames;
		public uint AddressOfFunctions;
		public uint AddressOfNames;
		public uint AddressOfNameOrdinals;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_DATA_DIRECTORY
	{
		public uint VirtualAddress;
		public uint Size;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_FILE_HEADER
	{
		public ushort Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort SizeOfOptionalHeader;
		public ushort Characteristics;
	}

	public enum MachineType : ushort
	{
		Native = 0,
		I386 = 0x014c,
		Itanium = 0x0200,
		x64 = 0x8664
	}

	public enum MagicType : ushort
	{
		IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
		IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct IMAGE_NT_HEADERS32
	{
		[FieldOffset(4)]
		public IMAGE_FILE_HEADER FileHeader;

		[FieldOffset(24)]
		public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct IMAGE_NT_HEADERS64
	{
		[FieldOffset(4)]
		public IMAGE_FILE_HEADER FileHeader;

		[FieldOffset(24)]
		public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_OPTIONAL_HEADER32
	{
		public MagicType Magic;
		public byte MajorLinkerVersion;
		public byte MinorLinkerVersion;
		public uint SizeOfCode;
		public uint SizeOfInitializedData;
		public uint SizeOfUninitializedData;
		public uint AddressOfEntryPoint;
		public uint BaseOfCode;
		public uint BaseOfData;
		public uint ImageBase;
		public uint SectionAlignment;
		public uint FileAlignment;
		public ushort MajorOperatingSystemVersion;
		public ushort MinorOperatingSystemVersion;
		public ushort MajorImageVersion;
		public ushort MinorImageVersion;
		public ushort MajorSubsystemVersion;
		public ushort MinorSubsystemVersion;
		public uint Win32VersionValue;
		public uint SizeOfImage;
		public uint SizeOfHeaders;
		public uint CheckSum;
		public ushort Subsystem;
		public ushort DllCharacteristics;
		public uint SizeOfStackReserve;
		public uint SizeOfStackCommit;
		public uint SizeOfHeapReserve;
		public uint SizeOfHeapCommit;
		public uint LoaderFlags;
		public uint NumberOfRvaAndSizes;
		public IMAGE_DATA_DIRECTORY ExportTable;
		public IMAGE_DATA_DIRECTORY ImportTable;
		public IMAGE_DATA_DIRECTORY ResourceTable;
		public IMAGE_DATA_DIRECTORY ExceptionTable;
		public IMAGE_DATA_DIRECTORY CertificateTable;
		public IMAGE_DATA_DIRECTORY BaseRelocationTable;
		public IMAGE_DATA_DIRECTORY Debug;
		public IMAGE_DATA_DIRECTORY Architecture;
		public IMAGE_DATA_DIRECTORY GlobalPtr;
		public IMAGE_DATA_DIRECTORY TLSTable;
		public IMAGE_DATA_DIRECTORY LoadConfigTable;
		public IMAGE_DATA_DIRECTORY BoundImport;
		public IMAGE_DATA_DIRECTORY IAT;
		public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
		public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
		public IMAGE_DATA_DIRECTORY Reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGE_OPTIONAL_HEADER64
	{
		public MagicType Magic;
		public byte MajorLinkerVersion;
		public byte MinorLinkerVersion;
		public uint SizeOfCode;
		public uint SizeOfInitializedData;
		public uint SizeOfUninitializedData;
		public uint AddressOfEntryPoint;
		public uint BaseOfCode;
		public ulong ImageBase;
		public uint SectionAlignment;
		public uint FileAlignment;
		public ushort MajorOperatingSystemVersion;
		public ushort MinorOperatingSystemVersion;
		public ushort MajorImageVersion;
		public ushort MinorImageVersion;
		public ushort MajorSubsystemVersion;
		public ushort MinorSubsystemVersion;
		public uint Win32VersionValue;
		public uint SizeOfImage;
		public uint SizeOfHeaders;
		public uint CheckSum;
		public ushort Subsystem;
		public ushort DllCharacteristics;
		public ulong SizeOfStackReserve;
		public ulong SizeOfStackCommit;
		public ulong SizeOfHeapReserve;
		public ulong SizeOfHeapCommit;
		public uint LoaderFlags;
		public uint NumberOfRvaAndSizes;
		public IMAGE_DATA_DIRECTORY ExportTable;
		public IMAGE_DATA_DIRECTORY ImportTable;
		public IMAGE_DATA_DIRECTORY ResourceTable;
		public IMAGE_DATA_DIRECTORY ExceptionTable;
		public IMAGE_DATA_DIRECTORY CertificateTable;
		public IMAGE_DATA_DIRECTORY BaseRelocationTable;
		public IMAGE_DATA_DIRECTORY Debug;
		public IMAGE_DATA_DIRECTORY Architecture;
		public IMAGE_DATA_DIRECTORY GlobalPtr;
		public IMAGE_DATA_DIRECTORY TLSTable;
		public IMAGE_DATA_DIRECTORY LoadConfigTable;
		public IMAGE_DATA_DIRECTORY BoundImport;
		public IMAGE_DATA_DIRECTORY IAT;
		public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
		public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
		public IMAGE_DATA_DIRECTORY Reserved;
	}
}
