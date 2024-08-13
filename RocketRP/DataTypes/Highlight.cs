using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Highlight
	{
		public int? frame { get; set; }
		public Name? CarName { get; set; }
		public Name? BallName { get; set; }
		public Name? GoalActorName { get; set; }
	}
}
