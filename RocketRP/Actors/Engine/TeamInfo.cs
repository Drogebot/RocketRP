using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class TeamInfo : Info
	{
		public int TeamIndex { get; set; }
		public int Score { get; set; }
		public string? TeamName { get; set; }
	}
}
