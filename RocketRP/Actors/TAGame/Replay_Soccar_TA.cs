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
		public IntProperty? TeamSize { get; set; }

		public IntProperty? UnfairTeamSize { get; set; }

		public BoolProperty? bUnfairBots { get; set; }

		public IntProperty? Team0Score { get; set; }

		public IntProperty? Team1Score { get; set; }

		public List<ScoredGoal> Goals { get; set; }

		public List<Highlight> Highlights { get; set; }

		public List<ReplayPlayerStats> PlayerStats { get; set; }
	}
}
