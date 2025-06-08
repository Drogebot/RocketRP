using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct PickupData2
	{
		public ObjectTarget<Car_TA>? Instigator { get; set; }
		public byte? PickedUp { get; set; }

		public PickupData2(ObjectTarget<Car_TA>? instigator, byte? pickedUp)
		{
			Instigator = instigator;
			PickedUp = pickedUp;
		}

		public static PickupData2 Deserialize(BitReader br)
		{
			var instigator = ObjectTarget<Car_TA>.Deserialize(br);
			var pickedUp = br.ReadByte();

			return new PickupData2(instigator, pickedUp);
		}

		public void Serialize(BitWriter bw)
		{
			Instigator!.Value.Serialize(bw);
			bw.Write(PickedUp);
		}
	}
}
