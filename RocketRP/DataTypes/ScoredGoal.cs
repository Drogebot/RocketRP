using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ScoredGoal
	{
		public IntProperty? frame { get; set; }

		public StrProperty? PlayerName { get; set; }

		public IntProperty? PlayerTeam { get; set; }
	}
}
