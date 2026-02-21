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

		public SceNpId(SceNpOnlineId handle, ulong opt, ulong reserved)
		{
			Handle = handle;
			Opt = opt;
			Reserved = reserved;
		}

		public static SceNpId Deserialize(BitReader br)
		{
			var handle = SceNpOnlineId.Deserialize(br);
			var opt = br.ReadUInt64();
			var reserved = br.ReadUInt64();

			return new SceNpId(handle, opt, reserved);
		}

		public void Serialize(BitWriter bw)
		{
			Handle!.Value.Serialize(bw);
			bw.Write(Opt);
			bw.Write(Reserved);
		}
	}
}
