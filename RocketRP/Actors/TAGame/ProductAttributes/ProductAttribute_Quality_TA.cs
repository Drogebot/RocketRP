using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_Quality_TA : ProductAttribute_TA
	{
		public EProductQuality? Quality { get; set; }

		public ProductAttribute_Quality_TA() { }

		public ProductAttribute_Quality_TA(EProductQuality quality)
		{
			Quality = quality;
		}

		public static ProductAttribute_Quality_TA DeserializeType(BitReader br, Replay replay)
		{
			throw new NotImplementedException();
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			throw new NotImplementedException();
		}
	}

	public enum EProductQuality : byte
	{
		EPQ_Common,
		EPQ_Uncommon,
		EPQ_Rare,
		EPQ_VeryRare,
		EPQ_Import,
		EPQ_Exotic,
		EPQ_BlackMarket,
		EPQ_Premium,
		EPQ_Limited,
		EPQ_Legacy,
		EPQ_END,
	}
}
