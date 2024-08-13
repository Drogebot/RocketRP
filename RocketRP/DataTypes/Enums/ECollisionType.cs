using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum ECollisionType : byte
	{
		COLLIDE_CustomDefault,
		COLLIDE_NoCollision,
		COLLIDE_BlockAll,
		COLLIDE_BlockWeapons,
		COLLIDE_TouchAll,
		COLLIDE_TouchWeapons,
		COLLIDE_BlockAllButWeapons,
		COLLIDE_TouchAllButWeapons,
		COLLIDE_BlockWeaponsKickable,
		COLLIDE_END,
	}
}
