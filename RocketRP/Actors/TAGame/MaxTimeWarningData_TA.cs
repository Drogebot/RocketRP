using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
    public class MaxTimeWarningData_TA : ReplicatedActor_ORS
	{
		public ulong EndGameEpochTime { get; set; }
		public ulong EndGameWarningEpochTime { get; set; }
	}
}
