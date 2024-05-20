using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.Enums
{
	public enum TieBreakDecision : byte
	{
		None,
		Goals,
		Shots,
		CoinToss,
		END,
	}
}
