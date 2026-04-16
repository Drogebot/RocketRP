using RocketRP.Actors.TAGame;

namespace RocketRP.DataTypes
{
	public struct PickupInfo_TA
	{
		[FixedArraySize(3)]
		public ObjectTarget<SpecialPickup_TA>[] AvailablePickups { get; set; }
		public bool bItemsArePreview { get; set; }

		public PickupInfo_TA(ObjectTarget<SpecialPickup_TA>[] availablePickups, bool bItemsArePreview)
		{
			AvailablePickups = availablePickups;
			this.bItemsArePreview = bItemsArePreview;
		}

		public static PickupInfo_TA Deserialize(BitReader br)
		{
			var availablePickups = new ObjectTarget<SpecialPickup_TA>[3]
			{
				ObjectTarget<SpecialPickup_TA>.Deserialize(br),
				ObjectTarget<SpecialPickup_TA>.Deserialize(br),
				ObjectTarget<SpecialPickup_TA>.Deserialize(br)
			};
			var bItemsArePreview = br.ReadBit();

			return new PickupInfo_TA(availablePickups, bItemsArePreview);
		}

		public readonly void Serialize(BitWriter bw)
		{
			AvailablePickups![0].Serialize(bw);
			AvailablePickups![1].Serialize(bw);
			AvailablePickups![2].Serialize(bw);
			bw.Write(bItemsArePreview);
		}
	}
}
