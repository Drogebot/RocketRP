using System.Text.Json.Serialization;

namespace RocketRP
{
	public struct ObjectType
	{
		public string Type { get; set; }
		[JsonIgnore]
		public uint FilePosition { get; set; }
		public uint ObjectIndex { get; set; }

		public static ObjectType Deserialize(BinaryReader br)
		{
			var ot = new ObjectType
			{
				Type = br.ReadString()!,
				FilePosition = br.ReadUInt32(),
				ObjectIndex = br.ReadUInt32()
			};

			return ot;
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(Type);
			bw.Write(FilePosition);
			bw.Write(ObjectIndex);
		}
	}
}
