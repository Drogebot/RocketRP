using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class DebugString
	{
		public uint FrameNumber { get; set; }
		public string Username { get; set; }
		public string Text { get; set; }

		public static DebugString Deserialize(BinaryReader br)
		{
			var debugString = new DebugString();
			debugString.FrameNumber = br.ReadUInt32();
			debugString.Username = "".Deserialize(br);
			debugString.Text = "".Deserialize(br);

			return debugString;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(FrameNumber);
			Username.Serialize(bw);
			Text.Serialize(bw);
		}
	}
}
