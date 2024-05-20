using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.ProjectX
{
	public class GRI_X : GameReplicationInfo
	{
		public string MatchGuid { get; set; }
		public bool bGameStarted { get; set; }
		public GameServerID GameServerID { get; set; }	// The type was changed from long to string at some point
		public ArrayProperty<Reservation> Reservations { get; set; } = new ArrayProperty<Reservation>(0x8);
		public string ReplicatedServerRegion { get; set; }
		public int ReplicatedGameMutatorIndex { get; set; }
		public int ReplicatedGamePlaylist { get; set; }


		// These are old properties that were removed
		public bool bGameEnded { get; set; }
	}
}
