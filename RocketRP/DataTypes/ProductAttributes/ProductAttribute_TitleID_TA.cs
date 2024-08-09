using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes.TAGame
{
	public class ProductAttribute_TitleID_TA : ProductAttribute_TA
	{
		public string Title { get; set; }

		public ProductAttribute_TitleID_TA(string title)
		{
			Title = title;
		}

		public static ProductAttribute_TitleID_TA DeserializeType(BitReader br, Replay replay)
		{
			string title = br.ReadString();

			return new ProductAttribute_TitleID_TA(title);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.Write(Title);
		}
	}
}
