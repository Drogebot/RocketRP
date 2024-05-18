﻿namespace RocketRP
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
		public List<string> Levels { get; set; }
		public List<KeyFrame> KeyFrames { get; set; }
		public List<DebugString> DebugStrings { get; set; }
		public List<Tickmark> Tickmarks { get; set; }
		public List<string> Packages { get; set; }
		public List<string> Objects { get; set; }
		public List<string> Names { get; set; }
		public Dictionary<string, int> ClassIndexes { get; set; }

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

			if(8 + replay.Part1Length != br.BaseStream.Position)
			{
				Console.WriteLine("Warning: Part1Length is not equal to the current position!");
			}

			replay.Part2Length = br.ReadUInt32();
			replay.Part2CRC = br.ReadUInt32();

			var numLevels = br.ReadInt32();
			replay.Levels = new List<string>(numLevels);
			for (int i = 0; i < numLevels; i++)
			{
				replay.Levels.Add(br.ReadString2());
			}

			var numKeyFrames = br.ReadInt32();
			replay.KeyFrames = new List<KeyFrame>(numKeyFrames);
			for(int i = 0; i < numKeyFrames; i++)
			{
				replay.KeyFrames.Add(KeyFrame.Deserialize(br));
			}

			// Skip the NetworkStream for now because we require the data after it to parse the NetworkStream
			var netStreamPosition = br.BaseStream.Position;
			var netStreamLength = br.ReadUInt32();
			br.BaseStream.Seek(netStreamLength, SeekOrigin.Current);

			var numDebugStrings = br.ReadInt32();
			replay.DebugStrings = new List<DebugString>(numDebugStrings);
			for (int i = 0; i < numDebugStrings; i++)
			{
				replay.DebugStrings.Add(DebugString.Deserialize(br));
			}

			var numTickmarks = br.ReadInt32();
			replay.Tickmarks = new List<Tickmark>(numTickmarks);
			for (int i = 0; i < numTickmarks; i++)
			{
				replay.Tickmarks.Add(Tickmark.Deserialize(br));
			}

			var numPackages = br.ReadInt32();
			replay.Packages = new List<string>(numPackages);
			for (int i = 0; i < numPackages; i++)
			{
				replay.Packages.Add(br.ReadString2());
			}

			var numObjects = br.ReadInt32();
			replay.Objects = new List<string>(numObjects);
			for (int i = 0; i < numObjects; i++)
			{
				replay.Objects.Add(br.ReadString2());
			}

			var numNames = br.ReadInt32();
			replay.Names = new List<string>(numNames);
			for (int i = 0; i < numNames; i++)
			{
				replay.Names.Add(br.ReadString2());
			}

			var numClassIndexes = br.ReadInt32();
			replay.ClassIndexes = new Dictionary<string, int>(numClassIndexes);
			for (int i = 0; i < numClassIndexes; i++)
			{
				replay.ClassIndexes.Add(br.ReadString2(), br.ReadInt32());
			}

			return replay;
		}
	}
}
