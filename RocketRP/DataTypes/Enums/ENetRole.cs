using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum ENetRole : byte
	{
		ROLE_None,
		ROLE_SimulatedProxy,
		ROLE_AutonomousProxy,
		ROLE_Authority,
		ROLE_END,
	}
}
