using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_TitleID_TA : ProductAttribute_TA
	{
		public Name? TitleId { get; set; }

		public ProductAttribute_TitleID_TA() { }

		public ProductAttribute_TitleID_TA(string title)
		{
			TitleId = title;
		}

		public static ProductAttribute_TitleID_TA DeserializeType(BitReader br, Replay replay)
		{
			string title = br.ReadString();

			return new ProductAttribute_TitleID_TA(title);
		}

		public override void Serialize(BitWriter bw, Replay replay)
		{
			base.Serialize(bw, replay);

			bw.Write((string)TitleId);
		}
	}
}
