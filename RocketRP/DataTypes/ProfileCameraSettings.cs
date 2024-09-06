using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ProfileCameraSettings
	{
		public float? FOV { get; set; }
		public float? Height { get; set; }
		public float? Pitch { get; set; }
		public float? Distance { get; set; }
		public float? Stiffness { get; set; }
		public float? SwivelSpeed { get; set; }
		public float? TransitionSpeed { get; set; }

		public ProfileCameraSettings(float fov, float height, float pitch, float distance, float stiffness, float swivelSpeed, float transitionSpeed)
		{
			this.FOV = fov;
			this.Height = height;
			this.Pitch = pitch;
			this.Distance = distance;
			this.Stiffness = stiffness;
			this.SwivelSpeed = swivelSpeed;
			this.TransitionSpeed = transitionSpeed;
		}

		public static ProfileCameraSettings Deserialize(BitReader br, Replay replay)
		{
			var fov = br.ReadSingle();
			var height = br.ReadSingle();
			var pitch = br.ReadSingle();
			var distance = br.ReadSingle();
			var stiffness = br.ReadSingle();
			var swivelSpeed = br.ReadSingle();

			float transitionSpeed = default;
			if (replay.EngineVersion >= 868 && replay.LicenseeVersion >= 20) transitionSpeed = br.ReadSingle();

			return new ProfileCameraSettings(fov, height, pitch, distance, stiffness, swivelSpeed, transitionSpeed);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(FOV.Value);
			bw.Write(Height.Value);
			bw.Write(Pitch.Value);
			bw.Write(Distance.Value);
			bw.Write(Stiffness.Value);
			bw.Write(SwivelSpeed.Value);

			if(replay.EngineVersion >= 868 && replay.LicenseeVersion >= 20) bw.Write(TransitionSpeed.Value);
		}
	}
}
