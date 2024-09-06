using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public static class StringExtension
	{
		public static string Deserialize(this string value, BinaryReader br)
		{
			// TODO: This is the same implementation as BitReader's ReadString()
			var length = br.ReadInt32();
			if (length > 0)
			{
				var bytes = br.ReadBytes(length);
				value = CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(bytes, 0, length - 1);
			}
			else if (length < 0)
			{
				var bytes = br.ReadBytes(length * -2);
				value = Encoding.Unicode.GetString(bytes, 0, (length * -2) - 2);
			}
			else
			{
				value = string.Empty;
			}

			return value;
		}

		public static void Serialize(this string value, BinaryWriter bw)
		{
			if(value == string.Empty)
			{
				bw.Write(0);
				return;
			}

			// TODO: This is the same implementation as BitWriter's Write(string)
			var length = value.Length + 1;
			bool isUnicode = value.Any(c => c > 255);
			if (isUnicode)
			{
				length *= -1;
			}
			bw.Write(length);

			if (!isUnicode)
			{
				bw.Write(CodePagesEncodingProvider.Instance.GetEncoding(1252).GetBytes(value));
				bw.Write((byte)0);
			}
			else
			{
				bw.Write(Encoding.Unicode.GetBytes(value));
				bw.Write((byte)0);
				bw.Write((byte)0);
			}
		}
	}
}
