using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class GameEvent_KnockOut_TA : GameEvent_Soccar_TA
	{
		public float PodiumSpawnLocationZ { get; set; }
		public int PlayerLives { get; set; }
	}
}
