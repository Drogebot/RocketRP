using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_Blueprint_TA : ProductAttribute_TA
	{
		public int? ProductID { get; set; }
		public int? CachedBlueprintSeriesID { get; set; }

		public ProductAttribute_Blueprint_TA() { }

		public ProductAttribute_Blueprint_TA(int productID, int cachedBlueprintSeriesID)
		{
			ProductID = productID;
			CachedBlueprintSeriesID = cachedBlueprintSeriesID;
		}

		public static ProductAttribute_Blueprint_TA DeserializeType(BitReader br, Replay replay)
		{
			throw new NotImplementedException();
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			throw new NotImplementedException();
		}
	}
}
