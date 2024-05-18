namespace RocketRP
{
	public class Replay
	{
		private const uint CRC_SEED = 0xEFCBF201;

		private uint Part1Length;
		private uint Part1CRC;
		private uint Part2Length;
		private uint Part2CRC;

		public uint EngineVersion { get; set; }
		public uint LicenseeVersion { get; set; }
		public uint NetVersion { get; set; }
		public string ReplayClass { get; set; }
		public PropertyDictionary Properties { get; set; }

		public static Replay Deserialize(string filePath)
		{
			return Deserialize(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
		}

		public static Replay Deserialize(FileStream stream)
		{
			return Deserialize(new BinaryReader(stream));
		}

		public static Replay Deserialize(BinaryReader br)
		{
			var replay = new Replay();

			replay.Part1Length = br.ReadUInt32();
			replay.Part1CRC = br.ReadUInt32();
			replay.EngineVersion = br.ReadUInt32();
			replay.LicenseeVersion = br.ReadUInt32();
			if(replay.EngineVersion >= 868 && replay.LicenseeVersion >= 18)
			{
				replay.NetVersion = br.ReadUInt32();
			}
			replay.ReplayClass = br.ReadString2();

			replay.Properties = PropertyDictionary.Deserialize(br);

			if(replay.Part1Length + 8 != br.BaseStream.Position)
			{
				Console.WriteLine("Warning: Part1Length is not equal to the current position!");
			}

			replay.Part2Length = br.ReadUInt32();
			replay.Part2CRC = br.ReadUInt32();



			return replay;
		}
	}
}
