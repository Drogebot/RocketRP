namespace RocketRP.DataTypes
{
	/// This type only exists because GameMode was changed from EnumMax to byte at some point
	public struct GameMode
	{
		public byte Value { get; set; }

		public GameMode(byte value)
		{
			Value = value;
		}

		public static GameMode Deserialize(BitReader br, Replay replay)
		{
			byte value;
			if (replay.LicenseeVersion >= 12) value = br.ReadByte();
			else value = (byte)br.ReadUInt32(4);

			return new GameMode(value);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			if (replay.LicenseeVersion >= 12) bw.Write(Value);
			else bw.Write(Value, 4);
		}

		public static implicit operator GameMode(byte value) => new(value);
		public static implicit operator byte(GameMode value) => value.Value;
	}
}
