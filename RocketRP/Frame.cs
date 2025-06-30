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
		public List<ActorUpdate> ActorUpdates { get; set; } = [];

		public static Frame Deserialize(BitReader br, Replay replay, Dictionary<int, ActorUpdate> openChannels, bool keepSnapshot = false)
		{
			var frame = new Frame
			{
				Time = br.ReadSingle(),
				Delta = br.ReadSingle()
			};

			var lastFrame = replay.Frames.LastOrDefault();
			if (replay.Frames.Count > 0 && (frame.Time < lastFrame?.Time && frame.Time != 0 || frame.Delta < 0))
			{
				throw new Exception("Unexpected Frame Time or Delta, Reader is most likely lost");
			}

			while (br.ReadBit())
			{
				var actorUpdate = ActorUpdate.Deserialize(br, replay, openChannels, keepSnapshot);
				frame.ActorUpdates.Add(actorUpdate);

				if (actorUpdate.State == ChannelState.Open) openChannels.TryAdd(actorUpdate.ChannelId, actorUpdate);
				else if (actorUpdate.State == ChannelState.Close) openChannels.Remove(actorUpdate.ChannelId);
			}

			return frame;
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(Time);
			bw.Write(Delta);

			foreach (var actorUpdate in ActorUpdates)
			{
				bw.Write(true);
				actorUpdate.Serialize(bw, replay);
			}
			bw.Write(false);
		}
	}
}
