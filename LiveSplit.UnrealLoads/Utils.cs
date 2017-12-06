using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace LiveSplit.DXLoads
{
	internal static class Utils
	{
		public static byte[] ToBytes(this IntPtr ptr) => BitConverter.GetBytes((int)ptr);

		public static byte[] ToBytes(this Status s) => BitConverter.GetBytes((int)s);

		public static string ToHex(this IList<byte> bytes)
		{
			var sb = new StringBuilder(bytes.Count * 2);

			foreach (var b in bytes)
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		public static IntPtr ReadJMP(this Process process, IntPtr ptr)
		{
			if (process.ReadBytes(ptr, 1)[0] == 0xE9)
				return ptr + 1 + sizeof(int) + process.ReadValue<int>(ptr + 1);
			else
				return IntPtr.Zero;
		}

		public static void CopyMemory(this Process process, IntPtr src, IntPtr dest, int nbr)
		{
			var bytes = process.ReadBytes(src, nbr);
			process.WriteBytes(dest, bytes);
		}

		public static List<byte> ParseBytes(string str, out int[] offsetsArray)
		{
			var offsets = new List<int>();
			var bytes = new List<byte>(str.Length / 2);
			var isOffset = false;

			var enumerator = str.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var c = enumerator.Current;
				if (char.IsWhiteSpace(c))
					continue;

				if (c == '#')
				{
					isOffset = true;
					continue;
				}

				if (isOffset)
				{
					isOffset = false;
					offsets.Add(bytes.Count);
				}

				var c2 = '\0';
				if (enumerator.MoveNext())
				{
					var next = char.ToLowerInvariant(enumerator.Current);
					if (char.IsDigit(next) || (next >= 'a' && next <= 'f'))
						c2 = next;
				}

				var byteStr = new string(new char[] { c, c2 });
				var bytee = byte.Parse(byteStr, NumberStyles.HexNumber);
				bytes.Add(bytee);
			}

			offsetsArray = offsets.ToArray();
			return bytes;
		}

		public static List<byte> ParseBytes(string str)
		{
			int[] array;
			return ParseBytes(str, out array);
		}
	}
}
