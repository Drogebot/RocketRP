using System;
using System.Collections.Generic;
using System.IO;

namespace RocketRP
{
	public class SaveData<T> where T : Actors.Core.Object, new()
	{
		private const uint FOOSBALL = 0xF005BA11U;
		private const uint MAGIC = 0x7FFFFFFFU;
		private const uint OBJHEADER = 0xFFFFFFFFU;
		private const uint CRC_SEED = 0xEFCBF201;

		public SaveDataVersionInfo VersionInfo { get; set; } = new SaveDataVersionInfo { EngineVersion = 868, LicenseeVersion = 32, TypeVersion = 0 };
		public required T Properties { get; set; }
		public List<Actors.Core.Object> Objects { get; set; } = [];
		public List<ObjectType> ObjectTypes { get; set; } = [];

		private byte[] SaveDataData = [];

		public static SaveData<T> Deserialize(string filePath, bool isEncrypted = true, bool enforeCRC = false)
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var savedata = Deserialize(fs, isEncrypted, enforeCRC);
			fs.Close();
			return savedata;
		}

		public static SaveData<T> Deserialize(FileStream stream, bool isEncrypted = true, bool enforeCRC = false)
		{
			return Deserialize(new BinaryReader(stream), isEncrypted, enforeCRC);
		}

		public static SaveData<T> Deserialize(BinaryReader br, bool isEncrypted = true, bool enforeCRC = false)
		{
			var part1Length = (uint)br.BaseStream.Length;
			var part1CRC = 0U;
			if (isEncrypted)
			{
				part1Length = br.ReadUInt32();
				part1CRC = br.ReadUInt32();
			}

			var part1Pos = (uint)br.BaseStream.Position;

			var data = br.ReadBytes((int)part1Length);

			if (part1Pos + part1Length != br.BaseStream.Position)
			{
				var message = $"Part1Length({part1Length}) is not equal to the current position({br.BaseStream.Position - part1Pos})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var calculatedPart1CRC = isEncrypted ? Crc32.CalculateCRC(data, CRC_SEED) : 0;
			if (part1CRC != calculatedPart1CRC)
			{
				var message = $"Part1CRC({part1CRC:X8}) does not match data({calculatedPart1CRC:X8})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}

			if(isEncrypted) AES.DecryptData(ref data, part1Length);
			Console.WriteLine($"Data length: {data.Length}");

			// Start reading the decrypted data
			br = new BinaryReader(new MemoryStream(data));

			var foosball = br.ReadUInt32();	// 0xF005BA11
			var magic = br.ReadUInt32();	// 0x7FFFFFFF
			if (foosball != FOOSBALL || magic != MAGIC)
			{
				var message = $"Foosball({foosball:X8}) or Magic({magic:X8}) is not correct!";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var versionInfo = SaveDataVersionInfo.Deserialize(br);

			var savedataLength = br.ReadInt32();
			var savedata = br.ReadBytes(savedataLength - sizeof(int));
			Console.WriteLine($"SaveData Length: {savedataLength}");

			var numObjectTypes = br.ReadInt32();
			var objectTypes = new List<ObjectType>(numObjectTypes);
			for (var i = 0; i < numObjectTypes; i++)
			{
				objectTypes.Add(ObjectType.Deserialize(br));
			}

			// Start reading the savedata
			br = new BinaryReader(new MemoryStream(savedata));

			var objHeader = br.ReadUInt32();  // 0xFFFFFFFF
			if (objHeader != OBJHEADER)
			{
				var message = $"objHeader({objHeader:X8}) is not correct!";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var properties = new T();
			Actors.Core.Object.Deserialize(properties, br, versionInfo);

			var saveData = new SaveData<T>()
			{
				VersionInfo = versionInfo,
				Properties = properties,
				SaveDataData = savedata,
				Objects = new List<Actors.Core.Object>(numObjectTypes),
				ObjectTypes = objectTypes,
			};

			for (var i = 0; i < numObjectTypes; i++)
			{
				br.BaseStream.Seek(saveData.ObjectTypes[i].FilePosition - sizeof(int), SeekOrigin.Begin);
				objHeader = br.ReadUInt32();  // 0xFFFFFFFF
				if (objHeader != OBJHEADER)
				{
					var message = $"objHeader({objHeader:X8}) is not correct!";
					if (enforeCRC) throw new Exception(message);
					Console.WriteLine($"Warning: {message}!");
				}
				var objType = Type.GetType($"RocketRP.Actors.{saveData.ObjectTypes[i].Type}") ?? throw new NullReferenceException($"SaveData Object Class Type {saveData.ObjectTypes[i].Type} does not exist");
				var obj = (Actors.Core.Object?)Activator.CreateInstance(objType) ?? throw new MissingMethodException($"{objType.Name} does not have a parameterless constructor");
				Actors.Core.Object.Deserialize(obj, br, saveData.VersionInfo);
				saveData.Objects.Add(obj);
			}

			return saveData;
		}

		public void Serialize(string filePath, bool encrypt = true)
		{
			var stream = new MemoryStream();
			Serialize(stream, encrypt);
			var dirName = Path.GetDirectoryName(filePath);
			if (!string.IsNullOrEmpty(dirName)) Directory.CreateDirectory(dirName);
			var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
			fs.Write(stream.ToArray());
			fs.Close();
		}

		public void Serialize(MemoryStream stream, bool encrypt = true)
		{
			Serialize(new BinaryWriter(stream), encrypt);
		}

		public void Serialize(BinaryWriter outWriter, bool encrypt = true)
		{
			var bw = new BinaryWriter(new MemoryStream());

			bw.Write(FOOSBALL);
			bw.Write(MAGIC);
			VersionInfo.Serialize(bw);

			var part2Pos = bw.BaseStream.Position;
			bw.Write(0);	// SaveData length

			bw.Write(OBJHEADER);
			Actors.Core.Object.Serialize(Properties, bw, VersionInfo);

			ObjectTypes = new List<ObjectType>(Objects.Count);
			uint i = 0;
			foreach (var obj in Objects)
			{
				ObjectTypes.Add(new ObjectType
				{
					Type = obj.GetType().FullName!.Replace("RocketRP.Actors.", ""),
					ObjectIndex = i++,
					FilePosition = (uint)(bw.BaseStream.Position - part2Pos)
				});
				bw.Write(OBJHEADER);
				Actors.Core.Object.Serialize(obj, bw, VersionInfo);
			}

			var savedataLength = bw.BaseStream.Position - part2Pos;
			bw.BaseStream.Seek(part2Pos, SeekOrigin.Begin);
			bw.Write((uint)savedataLength);
			bw.BaseStream.Seek(savedataLength - sizeof(uint), SeekOrigin.Current);

			bw.Write(ObjectTypes.Count);
			foreach (var objType in ObjectTypes)
			{
				objType.Serialize(bw);
			}

			var data = ((MemoryStream)bw.BaseStream).ToArray();

			if (encrypt)
			{
				AES.EncryptData(ref data, (uint)data.Length);
				outWriter.Write((uint)data.Length);
				outWriter.Write(Crc32.CalculateCRC(data, CRC_SEED));
			}

			outWriter.Write(data);
		}
	}

	public struct SaveDataVersionInfo : IFileVersionInfo
	{
		public int EngineVersion { get; set; }
		public int LicenseeVersion { get; set; }
		public int TypeVersion { get; set; }

		public static SaveDataVersionInfo Deserialize(BinaryReader br)
		{
			return new SaveDataVersionInfo
			{
				EngineVersion = br.ReadInt32(),
				LicenseeVersion = br.ReadInt32(),
				TypeVersion = br.ReadInt32()
			};
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(EngineVersion);
			bw.Write(LicenseeVersion);
			bw.Write(TypeVersion);
		}
	}
}
