using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CrowdActor_TA : Actor
	{
		public override bool HasInitialPosition => false;

		public int ReplicatedRoundCountDownNumber { get; set; }
		public int ReplicatedCountDownNumber { get; set; }
		public ObjectTarget ReplicatedOneShotSound { get; set; }
		public float ModifiedNoise { get; set; }
		public ObjectTarget GameEvent { get; set; }
	}
}
