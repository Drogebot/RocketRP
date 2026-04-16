using RocketRP.Actors.Engine;

namespace RocketRP.Actors.TAGame
{
	public class PlayerStart_Platform_TA : PlayerStart
	{
		public override bool HasInitialPosition => false;

		public bool bActive { get; set; }
	}
}
