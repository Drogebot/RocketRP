using RocketRP.Actors.Engine;
using RocketRP.DataTypes.Enums;

namespace RocketRP.Actors.TAGame
{
	public class BallKeepUpComponent_TA : ReplicatedActor_ORS
	{
		public EKeepUpState KeepUpState { get; set; }
		public ObjectTarget<Ball_TA> BallOwner { get; set; }
		public int Score { get; set; }
	}
}
