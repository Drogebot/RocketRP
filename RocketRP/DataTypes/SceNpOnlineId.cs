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
		//public ArrayProperty<ulong>? Data { get => _Data.HasValue ? _Data : new ArrayProperty<ulong>(2); set => _Data = value; }
		//private ArrayProperty<ulong>? _Data;
		public byte? Term { get; set; }
		public byte[]? Dummy { get; set; }
		//public ArrayProperty<byte>? Dummy { get => _Dummy.HasValue ? _Dummy : new ArrayProperty<byte>(3); set => _Dummy = value; }
		//private ArrayProperty<byte>? _Dummy;
	}
}
