using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public static class BinaryReaderExtensions
	{
		public static string ReadString2(this BinaryReader br)
		{
			// TODO: This is the same implementation as BitReader's ReadString. Try to consolidate someday
			var length = br.ReadInt32();
			if (length > 0)
			{
				var bytes = br.ReadBytes(length);
				return Encoding.ASCII.GetString(bytes, 0, length - 1);
			}
			else if (length < 0)
			{
				var bytes = br.ReadBytes(length * -2);
				return Encoding.Unicode.GetString(bytes, 0, (length * -2) - 2);
			}

			return "";
		}
	}
}
