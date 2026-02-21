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
		[FixedArraySize(2)]
		public ulong?[]? Data { get; set; }
		public byte? Term { get; set; }
		[FixedArraySize(3)]
		public byte?[]? Dummy { get; set; }

		public SceNpOnlineId(ulong?[]? data, byte? temp, byte?[]? dummy)
		{
			Data = data;
			Term = temp;
			Dummy = dummy;
		}

		public static SceNpOnlineId Deserialize(BitReader br)
		{
			ulong?[] data = [
				br.ReadUInt64(),
				br.ReadUInt64()
			];

			return new SceNpOnlineId(data, null, null);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(Data![0]);
			bw.Write(Data![1]);
		}
	}
}
