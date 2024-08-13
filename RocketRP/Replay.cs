using RocketRP.Actors.TAGame;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RocketRP
{
	public class Replay
	{
		private const uint CRC_SEED = 0xEFCBF201;
		private byte[] NetStreamData;

		public uint Part1Length { get; set; }
		public uint Part1CRC { get; set; }
		[JsonIgnore]
		public ReplayVersionInfo VersionInfo = new ReplayVersionInfo { EngineVersion = 868, LicenseeVersion = 32, NetVersion = 10 };
		public uint EngineVersion { get => VersionInfo.EngineVersion; set => VersionInfo.EngineVersion = value; }
		public uint LicenseeVersion { get => VersionInfo.LicenseeVersion; set => VersionInfo.LicenseeVersion = value; }
		public uint NetVersion { get => VersionInfo.NetVersion; set => VersionInfo.NetVersion = value; }
		public string ReplayClass { get; set; } = "TAGame.Replay_Soccar_TA";
		public Replay_TA Properties { get; set; }
		public uint Part2Length { get; set; }
		public uint Part2CRC { get; set; }
		public List<string> Levels { get; set; } = new List<string>();
		public List<Frame> Frames { get; set; } = new List<Frame>();
		public List<KeyFrame> KeyFrames { get; set; } = new List<KeyFrame>();
		public List<DebugString> DebugStrings { get; set; } = new List<DebugString>();
		public List<Tickmark> Tickmarks { get; set; } = new List<Tickmark>();
		public List<string> Packages { get; set; } = new List<string>();
		public List<string> Objects { get; set; } = new List<string>();
		public List<string> Names { get; set; } = new List<string>();
		public Dictionary<string, int> ClassIndexes { get; set; } = new Dictionary<string, int>();
		public List<ClassNetCache> ClassNetCaches { get; set; } = new List<ClassNetCache>();
		public uint Unknown { get; set; } = 0;

		public int MaxChannels { get => Properties.MaxChannels ?? 1023; }
		public int Changelist { get => Properties.Changelist ?? 0; }
		public Dictionary<string, ClassNetCache> ClassNetCacheByName;

		public static Replay Deserialize(string filePath, bool parseNetstream = true, bool enforeCRC = false)
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var replay = Deserialize(fs, parseNetstream, enforeCRC);
			fs.Close();
			return replay;
		}

		public static Replay Deserialize(FileStream stream, bool parseNetstream = true, bool enforeCRC = false)
		{
			return Deserialize(new BinaryReader(stream), parseNetstream, enforeCRC);
		}

		public static Replay Deserialize(BinaryReader br, bool parseNetstream = true, bool enforeCRC = false)
		{
			var replay = new Replay();

			replay.Part1Length = br.ReadUInt32();
			replay.Part1CRC = br.ReadUInt32();
			var part1Pos = (uint)br.BaseStream.Position;

			replay.VersionInfo = ReplayVersionInfo.Deserialize(br);
			replay.ReplayClass = "".Deserialize(br);

			replay.Properties = (Replay_TA)Activator.CreateInstance(Type.GetType($"RocketRP.Actors.{replay.ReplayClass}"));
			Actors.Core.Object.Deserialize(replay.Properties, br, replay.VersionInfo);

			if(part1Pos + replay.Part1Length != br.BaseStream.Position)
			{
				var message = $"Part1Length({replay.Part1Length}) is not equal to the current position({br.BaseStream.Position - part1Pos})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var calculatedPart1CRC = CalculateCRC(br, part1Pos, part1Pos + replay.Part1Length);
			if (replay.Part1CRC != calculatedPart1CRC)
			{
				var message = $"Part1CRC({replay.Part1CRC:X8}) does not match data({calculatedPart1CRC:X8})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}

			if (!parseNetstream) return replay;

			replay.Part2Length = br.ReadUInt32();
			replay.Part2CRC = br.ReadUInt32();
			var part2Pos = (uint)br.BaseStream.Position;

			var numLevels = br.ReadInt32();
			replay.Levels = new List<string>(numLevels);
			for (int i = 0; i < numLevels; i++)
			{
				replay.Levels.Add("".Deserialize(br));
			}

			var numKeyFrames = br.ReadInt32();
			replay.KeyFrames = new List<KeyFrame>(numKeyFrames);
			for(int i = 0; i < numKeyFrames; i++)
			{
				replay.KeyFrames.Add(KeyFrame.Deserialize(br));
			}

			// Skip the NetworkStream for now because we require the data after it to parse the NetworkStream
			var netStreamLength = br.ReadInt32();
			replay.NetStreamData = br.ReadBytes(netStreamLength);

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
				replay.Packages.Add("".Deserialize(br));
			}

			var numObjects = br.ReadInt32();
			replay.Objects = new List<string>(numObjects);
			for (int i = 0; i < numObjects; i++)
			{
				replay.Objects.Add("".Deserialize(br));
			}

			var numNames = br.ReadInt32();
			replay.Names = new List<string>(numNames);
			for (int i = 0; i < numNames; i++)
			{
				replay.Names.Add("".Deserialize(br));
			}

			var numClassIndexes = br.ReadInt32();
			replay.ClassIndexes = new Dictionary<string, int>(numClassIndexes);
			for (int i = 0; i < numClassIndexes; i++)
			{
				replay.ClassIndexes.Add("".Deserialize(br), br.ReadInt32());
			}

			var numClassNetCaches = br.ReadInt32();
			replay.ClassNetCaches = new List<ClassNetCache>(numClassNetCaches);
			for (int i = 0; i < numClassNetCaches; i++)
			{
				var classNetCache = ClassNetCache.Deserialize(br);

				// Check and fix for duplicate Objects, rare occurance but it can happen it seems. Somehow ClassIndexes does have the correct value but Objects doesn't (see `128ed.replay`)
				var dupe = replay.ClassNetCaches.Find(c => replay.Objects[c.ObjectIndex] == replay.Objects[classNetCache.ObjectIndex]);
				if (dupe != null)
				{
					var realObjectName = replay.ClassIndexes.Where(kvp => kvp.Value == classNetCache.ObjectIndex).First().Key;
					replay.Objects[classNetCache.ObjectIndex] = realObjectName;
					foreach(var prop in classNetCache.Properties)
					{
						replay.Objects[prop.ObjectIndex] = $"{realObjectName}:{replay.Objects[prop.ObjectIndex].Split(":").Last()}";
					}
				}

				classNetCache.CalculateParent(replay);
				replay.ClassNetCaches.Add(classNetCache);
			}

			if(replay.VersionInfo.NetVersion >= 10)
			{
				replay.Unknown = br.ReadUInt32();
			}

			if (part2Pos + replay.Part2Length != br.BaseStream.Position)
			{
				var message = $"Part2Length({replay.Part2Length}) is not equal to the current position({br.BaseStream.Position - part2Pos})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var calculatedPart2CRC = CalculateCRC(br, part2Pos, part2Pos + replay.Part2Length);
			if (replay.Part2CRC != calculatedPart2CRC)
			{
				var message = $"Part2CRC({replay.Part2CRC:X8}) does not match data({calculatedPart2CRC:X8})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}

			replay.DeserializeNetStream();

			return replay;
		}

		public void DeserializeNetStream()
		{
			if(ClassNetCacheByName == null) ClassNetCacheByName = ClassNetCaches.ToDictionary(c => Objects[c.ObjectIndex], c => c);
			var openChannels = new Dictionary<int, ActorUpdate>();

			Frames = new List<Frame>();
			var br = new BitReader(NetStreamData);

			while (br.Position < br.Length - 64)
			{
				var frame = Frame.Deserialize(br, this, openChannels);
				Frames.Add(frame);
			}
		}
		public void Serialize(string filePath)
		{
			var stream = new MemoryStream();
			Serialize(stream);
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
			fs.Write(stream.ToArray());
			fs.Close();
		}

		public void Serialize(MemoryStream stream)
		{
			Serialize(new BinaryWriter(stream));
		}

		public void Serialize(BinaryWriter bw)
		{
			if (ClassNetCacheByName == null) ClassNetCacheByName = ClassNetCaches.ToDictionary(c => Objects[c.ObjectIndex], c => c);
			foreach (var classNetCache in ClassNetCaches)
			{
				classNetCache.CalculateParent(this);
			}

			// These will be overwritten once we know their values
			bw.Write(0U);   // Part1Length
			bw.Write(0U);   // Part1CRC
			var part1Pos = (uint)bw.BaseStream.Position;

			VersionInfo.Serialize(bw);
			ReplayClass.Serialize(bw);
			Actors.Core.Object.Serialize(Properties, bw, VersionInfo);

			// Overwrite Part1Length and Part1CRC
			var curPos = bw.BaseStream.Position;
			Part1Length = (uint)(curPos - part1Pos);
			bw.BaseStream.Seek(part1Pos - 2 * sizeof(uint), SeekOrigin.Begin);
			bw.Write(Part1Length);
			Part1CRC = CalculateCRC(bw, part1Pos, part1Pos + Part1Length);
			bw.Write(Part1CRC);
			bw.BaseStream.Seek(Part1Length, SeekOrigin.Current);

			// These will be overwritten once we know their values
			bw.Write(0U);   // Part2Length
			bw.Write(0U);   // Part2CRC
			var part2Pos = (uint)bw.BaseStream.Position;

			bw.Write(Levels.Count);
			foreach (var level in Levels)
			{
				level.Serialize(bw);
			}

			bw.Write(KeyFrames.Count);
			foreach (var keyFrame in KeyFrames)
			{
				keyFrame.Serialize(bw);
			}

			NetStreamData = SerializeNetStream();
			bw.Write(NetStreamData.Length);
			bw.Write(NetStreamData);

			bw.Write(DebugStrings.Count);
			foreach (var debugString in DebugStrings)
			{
				debugString.Serialize(bw);
			}

			bw.Write(Tickmarks.Count);
			foreach (var tickmark in Tickmarks)
			{
				tickmark.Serialize(bw);
			}

			bw.Write(Packages.Count);
			foreach (var package in Packages)
			{
				package.Serialize(bw);
			}

			bw.Write(Objects.Count);
			foreach (var obj in Objects)
			{
				obj.Serialize(bw);
			}

			bw.Write(Names.Count);
			foreach (var name in Names)
			{
				name.Serialize(bw);
			}

			bw.Write(ClassIndexes.Count);
			foreach (var kvp in ClassIndexes)
			{
				kvp.Key.Serialize(bw);
				bw.Write(kvp.Value);
			}

			bw.Write(ClassNetCaches.Count);
			foreach (var classNetCache in ClassNetCaches)
			{
				classNetCache.Serialize(bw);
			}

			if (VersionInfo.NetVersion >= 10)
			{
				bw.Write(Unknown);
			}

			// Overwrite Part2Length and Part2CRC
			curPos = bw.BaseStream.Position;
			Part2Length = (uint)(curPos - part2Pos);
			bw.BaseStream.Seek(part2Pos - 2 * sizeof(uint), SeekOrigin.Begin);
			bw.Write(Part2Length);
			Part2CRC = CalculateCRC(bw, part2Pos, part2Pos + Part2Length);
			bw.Write(Part2CRC);
			bw.BaseStream.Seek(Part2Length, SeekOrigin.Current);
		}

		public byte[] SerializeNetStream()
		{
			var bw = new BitWriter();
			foreach (var frame in Frames)
			{
				frame.Serialize(bw, this, null);
			}

			return bw.GetAllBytes();
		}

		public static uint CalculateCRC(BinaryReader brs, uint startPos, uint endPos)
		{
			var size = (int)(endPos - startPos);
			var stream = new MemoryStream(size);

			var curPos = brs.BaseStream.Position;
			brs.BaseStream.Seek(startPos, SeekOrigin.Begin);
			brs.BaseStream.CopyTo(stream, stream.Capacity);
			brs.BaseStream.Seek(curPos, SeekOrigin.Begin);

			var br = new BinaryReader(stream);
			br.BaseStream.Seek(0, SeekOrigin.Begin);

			return Crc32.CalculateCRC(br.ReadBytes(size), CRC_SEED);
		}

		public static uint CalculateCRC(BinaryWriter bws, uint startPos, uint endPos)
		{
			var size = (int)(endPos - startPos);
			var stream = new MemoryStream(size);

			var curPos = bws.BaseStream.Position;
			bws.BaseStream.Seek(startPos, SeekOrigin.Begin);
			bws.BaseStream.CopyTo(stream, stream.Capacity);
			bws.BaseStream.Seek(curPos, SeekOrigin.Begin);

			var br = new BinaryReader(stream);
			br.BaseStream.Seek(0, SeekOrigin.Begin);

			return Crc32.CalculateCRC(br.ReadBytes(size), CRC_SEED);
		}
	}

	public struct ReplayVersionInfo : IFileVersionInfo
	{
		public uint EngineVersion { get; set; }
		public uint LicenseeVersion { get; set; }
		public uint TypeVersion { get; set; }
		public uint NetVersion { get => TypeVersion; set => TypeVersion = value; }

		public static ReplayVersionInfo Deserialize(BinaryReader br)
		{
			var rvi = new ReplayVersionInfo();
			rvi.EngineVersion = br.ReadUInt32();
			rvi.LicenseeVersion = br.ReadUInt32();
			if (rvi.EngineVersion >= 868 && rvi.LicenseeVersion >= 18) rvi.NetVersion = br.ReadUInt32();
			return rvi;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(EngineVersion);
			bw.Write(LicenseeVersion);
			if (EngineVersion >= 868 && LicenseeVersion >= 18) bw.Write(NetVersion);
		}
	}
}
