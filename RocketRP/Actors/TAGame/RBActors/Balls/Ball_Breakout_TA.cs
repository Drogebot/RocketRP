using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Ball_Breakout_TA : Ball_TA
	{
		public AppliedBreakoutDamage AppliedDamage { get; set; }
		public int DamageIndex { get; set; }
		public byte LastTeamTouch { get; set; }
	}
}
