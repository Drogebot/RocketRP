using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;

namespace RocketRP.Actors.TAGame
{
	public class BreakOutActor_Platform_TA : Actor
	{
		public override bool HasInitialPosition => false;

		public EBreakoutDamageState DefaultDamageState { get; set; }
		public BreakoutDamageState DamageState { get; set; }
		public bool bLockedDamageState { get; set; }
	}
}
