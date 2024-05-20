using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class SpecialPickup_BallVelcro_TA : SpecialPickup_TA
	{
		public float BreakTime { get; set; }
		public float AttachTime { get; set; }
		public bool bBroken { get; set; }
		public bool bHit { get; set; }
	}
}
