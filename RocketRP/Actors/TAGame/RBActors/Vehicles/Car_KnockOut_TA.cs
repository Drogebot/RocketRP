using RocketRP.DataTypes;

namespace RocketRP.Actors.TAGame
{
	public class Car_KnockOut_TA : Car_TA
	{
		public ObjectTarget<CarComponent_TA> UsedAttackComponent { get; set; }
		public ImpulseData ReplicatedImpulse { get; set; }
		public byte ReplicatedStateChanged { get; set; }
		public Name ReplicatedStateName { get; set; }
	}
}
