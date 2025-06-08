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
		public byte? Unknown1 { get; set; }
		public byte? Unknown2 { get; set; }

		public ReplicatedBoostData(byte? grantCount, byte? boostAmount, byte? unknown1, byte? unknown2)
		{
			GrantCount = grantCount;
			BoostAmount = boostAmount;
			Unknown1 = unknown1;
			Unknown2 = unknown2;
		}

		public static ReplicatedBoostData Deserialize(BitReader br)
		{
			var grantCount = br.ReadByte();
			var boostAmount = br.ReadByte();
			var unknown1 = br.ReadByte();
			var unknown2 = br.ReadByte();

			return new ReplicatedBoostData(grantCount, boostAmount, unknown1, unknown2);
		}

		public void Serialize(BitWriter bw)
		{
			bw.Write(GrantCount);
			bw.Write(BoostAmount);
			bw.Write(Unknown1);
			bw.Write(Unknown2);
		}
	}
}
