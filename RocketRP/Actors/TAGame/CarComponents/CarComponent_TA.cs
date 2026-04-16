using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_TA : Actor
	{
		public float ReplicatedActivityTime { get; set; }
		public ObjectTarget<Vehicle_TA> Vehicle { get; set; }
		public byte ReplicatedActive { get; set; }
	}
}
