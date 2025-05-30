using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class SpecialPickup_Football_TA : SpecialPickup_TA
	{
		public ObjectTarget<Ball_TA> WeldedBall { get; set; }
	}
}
