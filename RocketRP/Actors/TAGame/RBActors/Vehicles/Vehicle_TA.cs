using RocketRP.DataTypes.Enums;

namespace RocketRP.Actors.TAGame
{
	public class Vehicle_TA : RBActor_TA
	{
		public int PMCAnimIdx { get; set; }
		public int PodiumSpot { get; set; }
		public EInputRestriction InputRestriction { get; set; }
		public byte ReplicatedSteer { get; set; }
		public byte ReplicatedThrottle { get; set; }
		public bool bHasPostMatchCelebration { get; set; }
		public bool bPodiumMode { get; set; }
		public bool bReplicatedHandbrake { get; set; }
		public bool bDriving { get; set; }
	}
}
