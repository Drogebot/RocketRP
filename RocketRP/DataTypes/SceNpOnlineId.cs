using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct SceNpOnlineId
	{
		public ulong[]? Data { get; set; }
		public byte? Term { get; set; }
		public byte[]? Dummy { get; set; }
	}
}
