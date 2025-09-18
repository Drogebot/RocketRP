using RocketRP.Actors.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class Ball_Spawner_TA : Actor
	{
		public float SpawnDelaySeconds { get; set; }
		public ObjectTarget<Ball_TA> SpawnedBall { get; set; }
	}
}
