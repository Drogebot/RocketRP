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
		public ObjectTarget AvailablePickups { get; set; }
		public bool Unknown1 { get; set; }
		public bool bItemsArePreview { get; set; }

		public PickupInfo_TA(ObjectTarget availablePickups, bool unknown1, bool bItemsArePreview)
		{
			this.AvailablePickups = availablePickups;
			this.Unknown1 = unknown1;
			this.bItemsArePreview = bItemsArePreview;
		}

		public static PickupInfo_TA Deserialize(BitReader br)
		{
			var availablePickups = ObjectTarget.Deserialize(br);
			var unknown1 = br.ReadBit();
			var bItemsArePreview = br.ReadBit();

			return new PickupInfo_TA(availablePickups, unknown1, bItemsArePreview);
		}

		public void Serialize(BitWriter bw)
		{
			AvailablePickups.Serialize(bw);
			bw.Write(Unknown1);
			bw.Write(bItemsArePreview);
		}
	}
}
