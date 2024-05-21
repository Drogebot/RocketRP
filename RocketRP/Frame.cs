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
		public List<ActorUpdate> ActorUpdates { get; set; } = new List<ActorUpdate>();

		public static Frame Deserialize(BitReader br, Replay replay, Dictionary<int, ActorUpdate> openChannels)
		{
			var frame = new Frame();

			frame.Time = br.ReadSingle();
			frame.Delta = br.ReadSingle();

			while(br.ReadBit())
			{
				var actorUpdate = ActorUpdate.Deserialize(br, replay, openChannels);
				frame.ActorUpdates.Add(actorUpdate);

				if (actorUpdate.State == ChannelState.Open) openChannels.TryAdd(actorUpdate.ChannelId, actorUpdate);
				else if (actorUpdate.State == ChannelState.Close) openChannels.Remove(actorUpdate.ChannelId);
			}

			return frame;
		}
	}
}
