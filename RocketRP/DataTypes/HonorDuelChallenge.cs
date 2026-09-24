using System;
using System.Collections.Generic;
using System.Text;

namespace RocketRP.DataTypes
{
	public struct HonorDuelChallenge
	{
		public UniqueNetId Challenger { get; set; }
		public UniqueNetId Defender { get; set; }

		public HonorDuelChallenge(UniqueNetId challenger, UniqueNetId defender)
		{
			Challenger = challenger;
			Defender = defender;
		}

		public static HonorDuelChallenge Deserialize(BitReader br, Replay replay)
		{
			var challenger = UniqueNetId.Deserialize(br, replay);
			var defender = UniqueNetId.Deserialize(br, replay);

			return new HonorDuelChallenge(challenger, defender);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			Challenger.Serialize(bw, replay);
			Defender.Serialize(bw, replay);
		}
	}
}
