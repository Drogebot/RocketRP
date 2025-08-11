using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class SaveData<T> where T : Actors.Core.Object
	{
		private const uint FOOSBALL = 0xF005BA11U;
		private const uint MAGIC = 0x7FFFFFFFU;
		private const uint OBJHEADER = 0xFFFFFFFFU;
		private const uint CRC_SEED = 0xEFCBF201;
		private byte[]? Data;

		public uint Part1Length { get; set; }
		public uint Part1CRC { get; set; }
		public uint Foosball { get; set; } = FOOSBALL;
		public uint Magic { get; set; } = MAGIC;
		public SaveDataVersionInfo VersionInfo { get; set; } = new SaveDataVersionInfo { EngineVersion = 868, LicenseeVersion = 32, TypeVersion = 0 };
		public T Properties { get; set; } = null!;
		public List<Actors.Core.Object> Objects { get; set; } = [];
		public List<ObjectType> ObjectTypes { get; set; } = [];

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
			var savedata = new SaveData<T>();

			savedata.Part1Length = (uint)br.BaseStream.Length;
			if (isEncrypted)
			{
				savedata.Part1Length = br.ReadUInt32();
				savedata.Part1CRC = br.ReadUInt32();
			}

			var part1Pos = (uint)br.BaseStream.Position;

			savedata.Data = br.ReadBytes((int)savedata.Part1Length);

			if (part1Pos + savedata.Part1Length != br.BaseStream.Position)
			{
				var message = $"Part1Length({savedata.Part1Length}) is not equal to the current position({br.BaseStream.Position - part1Pos})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			var calculatedPart1CRC = isEncrypted ? Crc32.CalculateCRC(savedata.Data, CRC_SEED) : 0;
			if (savedata.Part1CRC != calculatedPart1CRC)
			{
				var message = $"Part1CRC({savedata.Part1CRC:X8}) does not match data({calculatedPart1CRC:X8})";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}

			if(isEncrypted) AES.DecryptData(ref savedata.Data, savedata.Part1Length);
			Console.WriteLine($"Data length: {savedata.Data.Length}");

			br = new BinaryReader(new MemoryStream(savedata.Data));

			savedata.Foosball = br.ReadUInt32();	// 0xF005BA11
			savedata.Magic = br.ReadUInt32();	// 0x7FFFFFFF
			if (savedata.Foosball != FOOSBALL || savedata.Magic != MAGIC)
			{
				var message = $"Foosball({savedata.Foosball:X8}) or Magic({savedata.Magic:X8}) is not correct!";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			savedata.VersionInfo = SaveDataVersionInfo.Deserialize(br);

			var part2Pos = (uint)br.BaseStream.Position;
			var savedataLength = br.ReadInt32();
			Console.WriteLine($"SaveData Length: {savedataLength}");

			// Skip all the object data for now
			br.BaseStream.Seek(part2Pos + savedataLength, SeekOrigin.Begin);

			var numObjectTypes = br.ReadInt32();
			savedata.ObjectTypes = new List<ObjectType>(numObjectTypes);
			for (var i = 0; i < numObjectTypes; i++)
			{
				savedata.ObjectTypes.Add(ObjectType.Deserialize(br));
			}

			// Go back to the object data and parse it
			br.BaseStream.Seek(part2Pos + sizeof(uint), SeekOrigin.Begin);

			var objHeader = br.ReadUInt32();  // 0xFFFFFFFF
			if (objHeader != OBJHEADER)
			{
				var message = $"objHeader({objHeader:X8}) is not correct!";
				if (enforeCRC) throw new Exception(message);
				Console.WriteLine($"Warning: {message}!");
			}
			savedata.Properties = (T?)Activator.CreateInstance(typeof(T)) ?? throw new MissingMethodException($"{typeof(T).Name} does not have a parameterless constructor");
			Actors.Core.Object.Deserialize(savedata.Properties, br, savedata.VersionInfo);

			savedata.Objects = new List<Actors.Core.Object>(numObjectTypes);
			for (var i = 0; i < numObjectTypes; i++)
			{
				br.BaseStream.Seek(part2Pos + savedata.ObjectTypes[i].FilePosition, SeekOrigin.Begin);
				objHeader = br.ReadUInt32();  // 0xFFFFFFFF
				if (objHeader != OBJHEADER)
				{
					var message = $"objHeader({objHeader:X8}) is not correct!";
					if (enforeCRC) throw new Exception(message);
					Console.WriteLine($"Warning: {message}!");
				}
				var objType = Type.GetType($"RocketRP.Actors.{savedata.ObjectTypes[i].Type}") ?? throw new NullReferenceException($"SaveData Object Class Type {savedata.ObjectTypes[i].Type} does not exist");
				var obj = (Actors.Core.Object?)Activator.CreateInstance(objType) ?? throw new MissingMethodException($"{objType.Name} does not have a parameterless constructor"); ;
				Actors.Core.Object.Deserialize(obj, br, savedata.VersionInfo);
				savedata.Objects.Add(obj);
			}

			return savedata;
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

			bw.Write(Foosball);
			bw.Write(Magic);
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

			Data = ((MemoryStream)bw.BaseStream).ToArray();

			if (encrypt)
			{
				AES.EncryptData(ref Data, (uint)Data.Length);
				outWriter.Write((uint)Data.Length);
				outWriter.Write(Crc32.CalculateCRC(Data, CRC_SEED));
			}

			outWriter.Write(Data);
		}
	}

	public struct SaveDataVersionInfo : IFileVersionInfo
	{
		public uint EngineVersion { get; set; }
		public uint LicenseeVersion { get; set; }
		public uint TypeVersion { get; set; }

		public static SaveDataVersionInfo Deserialize(BinaryReader br)
		{
			return new SaveDataVersionInfo
			{
				EngineVersion = br.ReadUInt32(),
				LicenseeVersion = br.ReadUInt32(),
				TypeVersion = br.ReadUInt32()
			};
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(EngineVersion);
			bw.Write(LicenseeVersion);
			bw.Write(TypeVersion);
		}
	}
}
