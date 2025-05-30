using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class PRI_KnockOut_TA : PRI_TA
	{
		public int MatchPlacement { get; set; }
		public int EliminationOrder { get; set; }
		public int Blocks { get; set; }
		public int Grabs { get; set; }
		public int Hits { get; set; }
		public int DamageCaused { get; set; }
		public int KnockoutDeaths { get; set; }
		public int Knockouts { get; set; }
		public bool bIsActiveMVP { get; set; }
		public bool bIsEliminated { get; set; }
	}
}
