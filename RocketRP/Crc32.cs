using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RocketRP
{
	public static class Crc32
	{
		private static uint[] CRCTable = [];

		public static void GenerateCRCTable()
		{
			CRCTable = new uint[256];
			for (uint i = 0; i < 256; i++)
			{
				uint c = i << 24;
				for (uint j = 8; j != 0; j--)
				{
					CRCTable[i] = c = (c & 0x80000000) > 0 ? (c << 1) ^ 0x04C11DB7 : (c << 1);
				}
			}
		}

		public static uint CalculateCRC(byte[] data, uint seed)
		{
			if (CRCTable.Length == 0) GenerateCRCTable();
			uint crc = ~seed;

			for (int i = 0; i < data.Length; i++)
			{
				crc = (crc << 8) ^ CRCTable[(crc >> 24) ^ data[i]];
			}

			return ~crc;
		}
	}
}
