using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class Frame
	{
		public float Time { get; set; }
		public float Delta { get; set; }
		public List<ActorUpdate> ActorUpdates { get; set; }

		public static Frame Deserialize(BitReader br, Replay replay)
		{
			var frame = new Frame();

			frame.Time = br.ReadSingle();
			frame.Delta = br.ReadSingle();

			while(br.ReadBit())
			{
				ActorUpdate.Deserialize(br, replay);
			}

			return frame;
		}
	}
}
