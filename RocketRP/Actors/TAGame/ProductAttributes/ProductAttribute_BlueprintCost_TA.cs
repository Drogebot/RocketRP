using System;

namespace RocketRP.Actors.TAGame
{
	public class ProductAttribute_BlueprintCost_TA : ProductAttribute_TA
	{
		public int Cost { get; set; }

		public ProductAttribute_BlueprintCost_TA() { }

		public ProductAttribute_BlueprintCost_TA(int cost)
		{
			Cost = cost;
		}

		public static ProductAttribute_BlueprintCost_TA DeserializeType(BitReader br, Replay replay)
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
