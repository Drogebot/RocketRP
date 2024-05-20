using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CarComponent_TA : Actor
	{
		public float ReplicatedActivityTime { get; set; }
		public ObjectTarget Vehicle { get; set; }
		public byte ReplicatedActive { get; set; }
	}
}
