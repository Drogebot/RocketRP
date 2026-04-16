using RocketRP.Actors.TAGame;

namespace RocketRP.DataTypes
{
	public struct PickupData
	{
		public ObjectTarget<Car_TA> Insigator { get; set; }
		public bool bPickedUp { get; set; }

		public PickupData(ObjectTarget<Car_TA> insigator, bool bPickedUp)
		{
			Insigator = insigator;
			this.bPickedUp = bPickedUp;
		}

		public static PickupData Deserialize(BitReader br)
		{
			var Insigator = ObjectTarget<Car_TA>.Deserialize(br);
			var bPickedUp = br.ReadBit();

			return new PickupData(Insigator, bPickedUp);
		}

		public readonly void Serialize(BitWriter bw)
		{
			Insigator.Serialize(bw);
			bw.Write(bPickedUp);
		}
	}
}
