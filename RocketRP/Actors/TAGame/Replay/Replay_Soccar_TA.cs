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
		public bool? bNoContest { get; set; }
		public bool? bForfeit { get; set; }
		public bool? bLocalPlayerAbandoned { get; set; }
		public int? PrimaryPlayerTeam { get; set; }
		public int? Team0Score { get; set; }
		public int? Team1Score { get; set; }
		public float? TotalSecondsPlayed { get; set; }
		public ulong? MatchStartEpoch { get; set; }
		public int? WinningTeam { get; set; }
		public ScoredGoal[]? Goals { get; set; }
		public Highlight[]? Highlights { get; set; }
		public ReplayPlayerStats[]? PlayerStats { get; set; }
	}
}
