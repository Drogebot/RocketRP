using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct PickupInfo_TA
	{
		[FixedArraySize(3)]
		public ObjectTarget<SpecialPickup_TA>?[]? AvailablePickups { get; set; }
		public bool? bItemsArePreview { get; set; }

		public PickupInfo_TA(ObjectTarget<SpecialPickup_TA>?[]? availablePickups, bool? bItemsArePreview)
		{
			AvailablePickups = availablePickups;
			this.bItemsArePreview = bItemsArePreview;
		}

		public static PickupInfo_TA Deserialize(BitReader br, Replay replay)
		{
			var availablePickups = new ObjectTarget<SpecialPickup_TA>?[3]
			{
				ObjectTarget<SpecialPickup_TA>.Deserialize(br, replay),
				ObjectTarget<SpecialPickup_TA>.Deserialize(br, replay),
				ObjectTarget<SpecialPickup_TA>.Deserialize(br, replay)
			};
			var bItemsArePreview = br.ReadBit();

			return new PickupInfo_TA(availablePickups, bItemsArePreview);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			AvailablePickups![0]!.Value.Serialize(bw, replay);
			AvailablePickups![1]!.Value.Serialize(bw, replay);
			AvailablePickups![2]!.Value.Serialize(bw, replay);
			bw.Write(bItemsArePreview);
		}
	}
}
