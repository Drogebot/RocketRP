using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class Ball_Spawner_TA : Actor
	{
		public float SpawnDelaySeconds { get; set; }
		public ObjectTarget<Ball_TA> SpawnedBall { get; set; }
	}
}
