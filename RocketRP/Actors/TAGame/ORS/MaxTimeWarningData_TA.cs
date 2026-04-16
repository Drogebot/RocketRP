using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class MaxTimeWarningData_TA : ReplicatedActor_ORS
	{
		public ulong EndGameEpochTime { get; set; }
		public ulong EndGameWarningEpochTime { get; set; }
	}
}
