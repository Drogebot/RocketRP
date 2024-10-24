using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Replay_Soccar_TA : Replay_TA
	{
		public int? TeamSize { get; set; }
		public int? UnfairTeamSize { get; set; }
		public bool? bUnfairBots { get; set; }
		public int? PrimaryPlayerTeam { get; set; }
		public int? Team0Score { get; set; }
		public int? Team1Score { get; set; }
		public float? TotalSecondsPlayed { get; set; }
		public ulong? MatchStartEpoch { get; set; }
		public int? WinningTeam { get; set; }
		public ArrayProperty<ScoredGoal>? Goals { get; set; }
		public ArrayProperty<Highlight>? Highlights { get; set; }
		public ArrayProperty<ReplayPlayerStats>? PlayerStats { get; set; }
	}
}
