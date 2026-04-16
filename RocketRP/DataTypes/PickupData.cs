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

		public static PickupData Deserialize(BitReader br, Replay replay)
		{
			var Insigator = ObjectTarget<Car_TA>.Deserialize(br, replay);
			var bPickedUp = br.ReadBit();

			return new PickupData(Insigator, bPickedUp);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			Insigator.Serialize(bw, replay);
			bw.Write(bPickedUp);
		}
	}
}
