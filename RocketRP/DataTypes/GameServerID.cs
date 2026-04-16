using System;

namespace RocketRP.DataTypes
{
	/// This type only exists because GameServerID was changed from long to string at some point
	public struct GameServerID
	{
		public string? Value { get; set; }

		public GameServerID(string? value)
		{
			Value = value;
		}

		public static GameServerID Deserialize(BitReader br, Replay	replay)
		{
			string? value;
			if(replay.Properties.Changelist >= 406184) value = br.ReadString();
			else value = br.ReadInt64().ToString();

			return new GameServerID(value);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			if (replay.Properties.Changelist >= 406184) bw.Write(Value);
			else bw.Write(Int64.Parse(Value!));
		}

		public static implicit operator GameServerID(string? value) => new(value);
		public static implicit operator string?(GameServerID value) => value.Value;
	}
}
