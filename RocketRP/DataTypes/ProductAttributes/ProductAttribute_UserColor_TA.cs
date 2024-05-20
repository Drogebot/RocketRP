using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.ProductAttributes
{
	public class ProductAttribute_UserColor_TA : ProductAttribute_TA
	{
		public uint Color { get; set; }

		public ProductAttribute_UserColor_TA(uint color)
		{
			Color = color;
		}

		public static new ProductAttribute_UserColor_TA Deserialize(BitReader br, Replay replay)
		{
			uint color = 0;
			if (replay.LicenseeVersion >= 23) color = br.ReadUInt32();
			else color = br.ReadUInt32FromBits(31);

			return new ProductAttribute_UserColor_TA(color);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			if (replay.LicenseeVersion >= 23) bw.Write(Color);
			else bw.WriteFixedBits(Color, 31);
		}
	}	
}
