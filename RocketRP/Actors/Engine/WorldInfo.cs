using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class WorldInfo : ZoneInfo
	{
		//public MusicTrackStruct ReplicatedMusicTrack { get; set; }	// This doesn't seem to get used and I don't feel like making that struct
		public float WorldGravityZ { get; set; }
		public ObjectTarget Pauser { get; set; }
		public float TimeDilation { get; set; }
		public bool bHighPriorityLoading { get; set; }
	}
}
