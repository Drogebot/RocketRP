using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ClientLoadoutOnlineDatas
	{
		public ClientLoadoutOnlineData[] Loadouts { get; set; }
		public bool bLoadoutSet { get; set; }
		public bool bDepricated { get; set; }

		public ClientLoadoutOnlineDatas(ClientLoadoutOnlineData[] Loadouts, bool bLoadoutSet, bool bDepricated)
		{
			this.Loadouts = Loadouts;
			this.bLoadoutSet = bLoadoutSet;
			this.bDepricated = bDepricated;
		}

		public static ClientLoadoutOnlineDatas Deserialize(BitReader br, Replay replay)
		{
			var loadouts = new ClientLoadoutOnlineData[2];
			loadouts[0] = ClientLoadoutOnlineData.Deserialize(br, replay);
			loadouts[1] = ClientLoadoutOnlineData.Deserialize(br, replay);
			var bLoadoutSet = br.ReadBit();
			var bDepricated = br.ReadBit();

			return new ClientLoadoutOnlineDatas(loadouts, bLoadoutSet, bDepricated);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			Loadouts[0].Serialize(bw, replay);
			Loadouts[1].Serialize(bw, replay);
			bw.Write(bLoadoutSet);
			bw.Write(bDepricated);
		}
	}
}
