using RocketRP.Actors.Engine;
using RocketRP.DataTypes;

namespace RocketRP.Actors.TAGame
{
	public class Team_TA : TeamInfo
	{
		public int Difficulty { get; set; }
		public ClubColorSet ClubColors { get; set; }
		public ReplicatedLogoData LogoData { get; set; }
		public ulong ClubID { get; set; }
		public string? CustomTeamName { get; set; }
		public ObjectTarget<GameEvent_Team_TA> GameEvent { get; set; }
	}
}
