using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This type doesn't quite match the original, but other parsers did it like this
	public struct PickupInfo_TA
	{
		public ObjectTarget[] AvailablePickups { get; set; }
		public bool bItemsArePreview { get; set; }

		public PickupInfo_TA(ObjectTarget[] availablePickups, bool bItemsArePreview)
		{
			this.AvailablePickups = availablePickups;
			this.bItemsArePreview = bItemsArePreview;
		}

		public static PickupInfo_TA Deserialize(BitReader br)
		{
			var availablePickups = new ObjectTarget[3];
			availablePickups[0] = ObjectTarget.Deserialize(br);
			availablePickups[1] = ObjectTarget.Deserialize(br);
			availablePickups[2] = ObjectTarget.Deserialize(br);
			var bItemsArePreview = br.ReadBit();

			return new PickupInfo_TA(availablePickups, bItemsArePreview);
		}

		public void Serialize(BitWriter bw)
		{
			AvailablePickups[0].Serialize(bw);
			AvailablePickups[1].Serialize(bw);
			AvailablePickups[2].Serialize(bw);
			bw.Write(bItemsArePreview);
		}
	}
}
