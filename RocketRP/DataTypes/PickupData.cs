using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct PickupData
	{
		public ObjectTarget<Car_TA>? Insigator { get; set; }
		public bool? bPickedUp { get; set; }

		public PickupData(ObjectTarget<Car_TA>? Insigator, bool? bPickedUp)
		{
			this.Insigator = Insigator;
			this.bPickedUp = bPickedUp;
		}

		public static PickupData Deserialize(BitReader br)
		{
			var Insigator = ObjectTarget<Car_TA>.Deserialize(br);
			var bPickedUp = br.ReadBit();

			return new PickupData(Insigator, bPickedUp);
		}

		public void Serialize(BitWriter bw)
		{
			Insigator!.Value.Serialize(bw);
			bw.Write(bPickedUp);
		}
	}
}
