using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_NoNotify_TA : ProductAttribute_TA
	{
		public ProductAttribute_NoNotify_TA() { }

		public static ProductAttribute_NoNotify_TA DeserializeType(BitReader br, Replay replay)
		{
			return new ProductAttribute_NoNotify_TA();
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);
		}
	}
}
