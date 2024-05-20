using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class PickupTimer_TA : CarComponent_TA
	{
		public int MaxTimeTillItem { get; set; }
		public int TimeTillItem { get; set; }
	}
}
