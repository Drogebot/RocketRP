using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public struct ObjectType
	{
		public string Type { get; set; }
		[JsonIgnore]
		public uint FilePosition { get; set; }
		[JsonIgnore]
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

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(Type);
			bw.Write(FilePosition);
			bw.Write(ObjectIndex);
		}
	}
}
