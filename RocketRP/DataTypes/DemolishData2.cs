using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct DemolishData2
	{
		public ObjectTarget Target { get; set; }

		public DemolishData DemolishData { get; set; }

		public DemolishData2(ObjectTarget target, DemolishData demolishData)
		{
			Target = target;
			DemolishData = demolishData;
		}

		public static DemolishData2 Deserialize(BitReader br, Replay replay)
		{
			var target = ObjectTarget.Deserialize(br);
			var demolishData = DemolishData.Deserialize(br, replay);

			return new DemolishData2(target, demolishData);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Target.Serialize(bw);
			DemolishData.Serialize(bw, replay);
		}
	}
}
