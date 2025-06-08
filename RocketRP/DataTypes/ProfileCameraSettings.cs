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

		public ProfileCameraSettings(float? fov, float? height, float? pitch, float? distance, float? stiffness, float? swivelSpeed, float? transitionSpeed)
		{
			FOV = fov;
			Height = height;
			Pitch = pitch;
			Distance = distance;
			Stiffness = stiffness;
			SwivelSpeed = swivelSpeed;
			TransitionSpeed = transitionSpeed;
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
			bw.Write(FOV);
			bw.Write(Height);
			bw.Write(Pitch);
			bw.Write(Distance);
			bw.Write(Stiffness);
			bw.Write(SwivelSpeed);

			if(replay.EngineVersion >= 868 && replay.LicenseeVersion >= 20) bw.Write(TransitionSpeed);
		}
	}
}
