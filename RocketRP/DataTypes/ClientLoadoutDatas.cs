using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ClientLoadoutDatas
	{
		[FixedArraySize(2)]
		public ClientLoadoutData?[]? Loadouts { get; set; }

		public ClientLoadoutDatas(ClientLoadoutData?[]? clientLoadouts)
		{
			Loadouts = clientLoadouts;
		}

		public static ClientLoadoutDatas Deserialize(BitReader br)
		{
			var loadouts = new ClientLoadoutData?[2]
			{
				ClientLoadoutData.Deserialize(br),
				ClientLoadoutData.Deserialize(br),
			};

			return new ClientLoadoutDatas(loadouts);
		}

		public void Serialize(BitWriter bw)
		{
			Loadouts![0]!.Value.Serialize(bw);
			Loadouts![1]!.Value.Serialize(bw);
		}
	}
}
