using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class PlayerStart_Platform_TA : PlayerStart
	{
		public override bool HasInitialPosition => false;

		public bool bActive { get; set; }
	}
}
