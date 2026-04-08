using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ReplicatedBoostData
	{
		public byte? GrantCount { get; set; }
		public byte? BoostAmount { get; set; }
		public byte? Unused1 { get; set; }
		public byte? Unused2 { get; set; }

		public ReplicatedBoostData(byte? grantCount, byte? boostAmount, byte? unused1, byte? unused2)
		{
			GrantCount = grantCount;
			BoostAmount = boostAmount;
			Unused1 = unused1;
			Unused2 = unused2;
		}

		public static ReplicatedBoostData Deserialize(BitReader br, Replay replay)
		{
			var grantCount = br.ReadByte();
			var boostAmount = br.ReadByte();
			var unused1 = br.ReadByte();
			var unused2 = br.ReadByte();

			return new ReplicatedBoostData(grantCount, boostAmount, unused1, unused2);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(GrantCount);
			bw.Write(BoostAmount);
			bw.Write(Unused1);
			bw.Write(Unused2);
		}
	}
}
