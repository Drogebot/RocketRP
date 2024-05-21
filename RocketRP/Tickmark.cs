using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class Tickmark
	{
		public string Type { get; set; }
		public uint Frame { get; set; }

		public static Tickmark Deserialize(BinaryReader br)
		{
			var tickmark = new Tickmark();
			tickmark.Type = "".Deserialize(br);
			tickmark.Frame = br.ReadUInt32();

			return tickmark;
		}

		public void Serialize(BinaryWriter bw)
		{
			Type.Serialize(bw);
			bw.Write(Frame);
		}
	}
}
