using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum ETieBreakDecision : byte
	{
		TBD_None,
		TBD_Goals,
		TBD_Shots,
		TBD_CoinToss,
		TBD_END,
	}
}
