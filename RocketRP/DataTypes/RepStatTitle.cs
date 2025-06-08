using RocketRP.Actors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type no longer exists inside Rocket League, so I have no clue what the official names are
	public struct RepStatTitle
	{
		public bool? Unknown1 { get; set; }
		public string? Name { get; set; }
		public ObjectTarget<ClassObject>? ObjectTarget { get; set; }
		public uint? Value { get; set; }

		public RepStatTitle(bool? unknown1, string? name, ObjectTarget<ClassObject>? objectTarget, uint? value)
		{
			Unknown1 = unknown1;
			Name = name;
			ObjectTarget = objectTarget;
			Value = value;
		}

		public static RepStatTitle Deserialize(BitReader br)
		{
			var unknown1 = br.ReadBit();
			var name = br.ReadString();
			var objectTarget = ObjectTarget<ClassObject>.Deserialize(br);
			var value = br.ReadUInt32();

			return new RepStatTitle(unknown1, name, objectTarget, value);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Unknown1);
			bw.Write(Name);
			ObjectTarget!.Value.Serialize(bw);
			bw.Write(Value);
		}
	}
}
