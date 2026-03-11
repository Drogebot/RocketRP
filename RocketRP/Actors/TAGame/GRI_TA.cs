using RocketRP.Actors.ProjectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class GRI_TA : GRI_X
	{
		public bool bAllowTargetFind { get; set; }


		// These are old properties that were removed
		public string? NewDedicatedServerIP { get; set; }
	}
}
