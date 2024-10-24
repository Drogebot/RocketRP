using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct SceNpId
	{
		public SceNpOnlineId? Handle { get; set; }
		public ulong? Opt { get; set; }
		public ulong? Reserved { get; set; }
		public OnlinePlatform? Platform { get; set; }
	}
}
