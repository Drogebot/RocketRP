namespace RocketRP
{
	public struct ReplayLogItem
	{
		public int frame { get; set; }
		public string? PlayerName { get; set; }
		public string? Text { get; set; }

		public static ReplayLogItem Deserialize(BinaryReader br)
		{
			var debugString = new ReplayLogItem
			{
				frame = br.ReadInt32(),
				PlayerName = br.ReadString(),
				Text = br.ReadString()
			};

			return debugString;
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(frame);
			bw.Write(PlayerName);
			bw.Write(Text);
		}
	}
}
