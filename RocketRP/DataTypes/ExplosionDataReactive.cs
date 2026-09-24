using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketRP.DataTypes
{
	public struct ExplosionDataReactive
	{
		public bool bPodiumExplosion { get; set; }
		public byte ScorerGoalNumber { get; set; }

		public ExplosionDataReactive(bool bPodiumExplosion, byte scorerGoalNumber)
		{
			this.bPodiumExplosion = bPodiumExplosion;
			ScorerGoalNumber = scorerGoalNumber;
		}

		public static ExplosionDataReactive Deserialize(BitReader br, Replay replay)
		{
			var bPodiumExplosion = br.ReadBit();
			var scorerGoalNumber = br.ReadByte();

			return new ExplosionDataReactive(bPodiumExplosion, scorerGoalNumber);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(bPodiumExplosion);
			bw.Write(ScorerGoalNumber);
		}
	}
}
