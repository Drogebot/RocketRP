using RocketRP.Actors.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
    public struct ClientLoadoutOnlineData
	{
		public List<List<ProductAttribute_TA>> Products { get; set; }

		public ClientLoadoutOnlineData(List<List<ProductAttribute_TA>> products)
		{
			Products = products;
		}

		public static ClientLoadoutOnlineData Deserialize(BitReader br, Replay replay)
		{
			var count = br.ReadByte();
			var products = new List<List<ProductAttribute_TA>>(count);
			for (int i = 0; i < count; i++)
			{
				var attributeCount = br.ReadByte();
				var attributes = new List<ProductAttribute_TA>(attributeCount);
				for (int a = 0; a < attributeCount; a++)
				{
					attributes.Add(ProductAttribute_TA.Deserialize(br, replay));
				}
				products.Add(attributes);
			}

			return new ClientLoadoutOnlineData(products);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write((byte)Products.Count);
			foreach (var product in Products)
			{
				bw.Write((byte)product.Count);
				foreach (var attribute in product)
				{
					attribute.Serialize(bw, replay);
				}
			}
		}
	}
}
