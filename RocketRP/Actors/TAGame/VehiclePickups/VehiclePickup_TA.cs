using RocketRP.Actors.Engine;
using RocketRP.DataTypes;

namespace RocketRP.Actors.TAGame
{
	public class VehiclePickup_TA : Actor
	{
		public bool bNoPickup { get; set; }
		public PickupData2 NewReplicatedPickupData { get; set; }
		public PickupData ReplicatedPickupData { get; set; }
	}
}
