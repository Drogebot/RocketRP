namespace RocketRP.Actors.TAGame
{
	public class GameEvent_Team_TA : GameEvent_TA
	{
		public bool bDisableQuickChat { get; set; }
		public bool bForfeit { get; set; }
		public bool bDisableMutingOtherTeam { get; set; }
		public int MaxTeamSize { get; set; }
	}
}
