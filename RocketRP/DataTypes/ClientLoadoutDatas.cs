using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ClientLoadoutDatas
	{
		public ClientLoadoutData[] Loadouts { get; set; }

		public ClientLoadoutDatas(ClientLoadoutData[] clientLoadouts)
		{
			Loadouts = clientLoadouts;
		}

		public static ClientLoadoutDatas Deserialize(BitReader br)
		{
			var loadouts = new ClientLoadoutData[2];
			loadouts[0] = ClientLoadoutData.Deserialize(br);
			loadouts[1] = ClientLoadoutData.Deserialize(br);

			return new ClientLoadoutDatas(loadouts);
		}

		public void Serialize(BitWriter bw)
		{
			Loadouts[0].Serialize(bw);
			Loadouts[1].Serialize(bw);
		}
	}
}
