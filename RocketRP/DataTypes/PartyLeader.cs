using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	// This struct only exists because UniqueNetId works differently when it's a TAGame.PRI_TA:PartyLeader...
	public struct PartyLeader
	{
		public UniqueNetId UniqueId { get; set; }

		public PartyLeader(UniqueNetId uniqueId)
		{
			this.UniqueId = uniqueId;
		}

		public static PartyLeader Deserialize(BitReader br, Replay replay)
		{
			var uniqueId = UniqueNetId.Deserialize2(br, replay, true);

			return new PartyLeader(uniqueId);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			UniqueId.Serialize2(bw, replay, true);
		}
	}
}
