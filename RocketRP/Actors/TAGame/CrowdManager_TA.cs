using RocketRP.Actors.Core;
using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class CrowdManager_TA : Actor
	{
		public override bool HasInitialPosition => false;

		public ObjectTarget<GameEvent_Soccar_TA> GameEvent { get; set; }
		public ObjectTarget<ClassObject> ReplicatedGlobalOneShotSound { get; set; }
	}
}
