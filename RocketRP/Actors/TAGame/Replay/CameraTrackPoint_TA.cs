using RocketRP.DataTypes;

namespace RocketRP.Actors.TAGame
{
	public class CameraTrackPoint_TA : Core.Object
	{
		public int frame { get; set; }
		public float Time { get; set; }
		public Vector Location { get; set; }
		public Rotator Rotation { get; set; }
		public float FOV { get; set; }
	}
}
