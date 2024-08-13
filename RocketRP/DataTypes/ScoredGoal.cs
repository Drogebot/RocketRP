using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ScoredGoal
	{
		public int? frame { get; set; }
		public string? PlayerName { get; set; }
		public int? PlayerTeam { get; set; }
	}
}
