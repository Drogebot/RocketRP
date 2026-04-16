using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class CrowdActor_TA : Actor
	{
		public override bool HasInitialPosition => false;

		public int ReplicatedRoundCountDownNumber { get; set; }
		public int ReplicatedCountDownNumber { get; set; }
		public ObjectTarget<ClassObject> ReplicatedOneShotSound { get; set; }
		public float ModifiedNoise { get; set; }
		public ObjectTarget<GameEvent_Soccar_TA> GameEvent { get; set; }
	}
}
