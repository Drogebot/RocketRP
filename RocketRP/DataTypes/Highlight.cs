using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Highlight
	{
		public IntProperty? frame { get; set; }

		public NameProperty? CarName { get; set; }

		public NameProperty? BallName { get; set; }

		public NameProperty? GoalActorName { get; set; }
	}
}
