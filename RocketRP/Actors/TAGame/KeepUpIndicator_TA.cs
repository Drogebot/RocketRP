using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class KeepUpIndicator_TA : Actor
	{
		public override bool HasInitialRotation => true;

		public ObjectTarget<BallKeepUpComponent_TA> ComponentOwner { get; set; }
	}
}
