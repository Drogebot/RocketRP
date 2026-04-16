namespace RocketRP.Actors.TAGame
{
	public class Ball_Haunted_TA : Ball_TA
	{
		public bool bIsBallBeamed { get; set; }
		public byte TotalActiveBeams { get; set; }
		public byte DeactivatedGoalIndex { get; set; }
		public byte LastTeamTouch { get; set; }
		public byte ReplicatedBeamBrokenValue { get; set; }
	}
}
