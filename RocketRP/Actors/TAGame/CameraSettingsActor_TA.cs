using RocketRP.Actors.Engine;
using RocketRP.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.TAGame
{
	public class CameraSettingsActor_TA : ReplicationInfo
	{

		public byte CameraYaw { get; set; }
		public byte CameraPitch { get; set; }
		public bool bUsingFreecam { get; set; }
		public bool bUsingBehindView { get; set; }
		public bool bUsingSecondaryCamera { get; set; }
		public ProfileCameraSettings ProfileSettings { get; set; }
		public ObjectTarget<PRI_TA> PRI { get; set; }


		// These are old properties that were removed
		public bool bResetCamera { get; set; }
		public bool bHoldMouseCamera { get; set; }
		public bool bMouseCameraToggleEnabled { get; set; }
		public bool bUsingSwivel { get; set; }
	}
}
