using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class RumblePickups_TA : Actor
	{
		public int PreviewTimeSeconds { get; set; }
		public int ConcurrentItemCount { get; set; }
		public PickupInfo_TA PickupInfo { get; set; }


		// These are old properties that were removed
		public ObjectTarget AttachedPickup { get; set; }
	}
}
