using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type is originally just a sort of string, but replays use an index into the replay's name table
	public struct Name
	{
		public int? NameIndex { get; set; }
		public string? Value { get; set; }

		public Name(int nameIndex, string value = "")
		{
			NameIndex = nameIndex;
			Value = value;
		}

		public static Name Deserialize(BinaryReader br)
		{
			return new Name
			{
				Value = "".Deserialize(br)
			};
		}

		public void Serialize(BinaryWriter bw)
		{
			Value.Serialize(bw);
		}

		public static Name Deserialize(BitReader br, Replay replay)
		{
			var nameIndex = br.ReadInt32();

			return new Name(nameIndex, replay.Names[nameIndex]);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write((int)NameIndex);
		}

		public static implicit operator Name(string value) => new Name { Value = value };
		public static implicit operator string(Name value) => value.Value;
	}
}
