using System;
using System.Collections.Generic;

namespace RocketRP.DataTypes
{
	/// This type is originally just a sort of string, but replays use an index into the replay's name table
	public struct Name
	{
		public string Value { get; set; }

		public Name(string value)
		{
			Value = value;
		}

		public static Name Deserialize(BinaryReader br)
		{
			return new Name
			{
				Value = br.ReadString()!
			};
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public static Name Deserialize(BitReader br, Replay replay)
		{
			var nameIndex = br.ReadInt32();

			return new Name(replay.Names[nameIndex]);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			if (Value is null) throw new NullReferenceException($"Value of {nameof(Name)} was null");
			var nameIndex = replay.Names.IndexOf(Value);
			if (nameIndex == -1) throw new KeyNotFoundException($"Name {Value} not found in {nameof(replay.Names)}");
			bw.Write(nameIndex);
		}

		public static implicit operator Name(string value) => new(value);
		public static implicit operator string(Name value) => value.Value;
	}
}
