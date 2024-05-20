using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct PickupData
	{
		public ObjectTarget Insigator { get; set; }
		public bool bPickedUp { get; set; }

		public PickupData(ObjectTarget Insigator, bool bPickedUp)
		{
			this.Insigator = Insigator;
			this.bPickedUp = bPickedUp;
		}

		public static PickupData Deserialize(BitReader br)
		{
			var Insigator = ObjectTarget.Deserialize(br);
			var bPickedUp = br.ReadBit();

			return new PickupData(Insigator, bPickedUp);
		}

		public void Serialize(BitWriter bw)
		{
			Insigator.Serialize(bw);
			bw.Write(bPickedUp);
		}
	}
}
