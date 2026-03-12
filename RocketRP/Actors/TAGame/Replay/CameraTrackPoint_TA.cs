using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RocketRP.Actors.TAGame
{
	public class CameraTrackPoint_TA : Core.Object
	{
		public int? frame { get; set; }
		public float? Time { get; set; }
		public Vector? Location { get; set; }
		public Rotator? Rotation { get; set; }
		public float? FOV { get; set; }
	}
}
