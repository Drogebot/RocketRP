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

			while (br.ReadBit())
			{
				var actorUpdate = ActorUpdate.Deserialize(br, replay, openChannels);
				frame.ActorUpdates.Add(actorUpdate);

				if (actorUpdate.State == ChannelState.Open) openChannels.TryAdd(actorUpdate.ChannelId, actorUpdate);
				else if (actorUpdate.State == ChannelState.Close) openChannels.Remove(actorUpdate.ChannelId);
			}

			return frame;
		}

		public void Serialize(BitWriter bw, Replay replay, Dictionary<int, ActorUpdate> openChannels)
		{
			bw.Write(Time);
			bw.Write(Delta);

			foreach (var actorUpdate in ActorUpdates)
			{
				bw.Write(true);
				actorUpdate.Serialize(bw, replay, openChannels);
			}
			bw.Write(false);
		}
	}
}
