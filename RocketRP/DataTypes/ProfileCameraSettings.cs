namespace RocketRP.DataTypes
{
	public struct ProfileCameraSettings
	{
		public float FOV { get; set; }
		public float Height { get; set; }
		public float Pitch { get; set; }
		public float Distance { get; set; }
		public float Stiffness { get; set; }
		public float SwivelSpeed { get; set; }
		public float TransitionSpeed { get; set; }
		public bool bUnconstrainRotation { get; set; }
		public bool bFreeLookSmoothing { get; set; }
		public float CameraAccelRate { get; set; }
		public float CameraDecelRate { get; set; }
		public float FreeLookSpeed { get; set; }

		public ProfileCameraSettings(float fov, float height, float pitch, float distance, float stiffness, float swivelSpeed, float transitionSpeed, bool bUnconstrainRotation, bool bFreeLookSmoothing, float cameraAccelRate, float cameraDecelRate, float freeLookSpeed)
		{
			FOV = fov;
			Height = height;
			Pitch = pitch;
			Distance = distance;
			Stiffness = stiffness;
			SwivelSpeed = swivelSpeed;
			TransitionSpeed = transitionSpeed;
			this.bUnconstrainRotation = bUnconstrainRotation;
			this.bFreeLookSmoothing = bFreeLookSmoothing;
			CameraAccelRate = cameraAccelRate;
			CameraDecelRate = cameraDecelRate;
			FreeLookSpeed = freeLookSpeed;
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
			if (replay.LicenseeVersion >= 20) transitionSpeed = br.ReadSingle();

			bool bUnconstrainRotation = default; bool bFreeLookSmoothing = default; float cameraAccelRate = default; float cameraDecelRate = default; float freeLookSpeed = default;
			if (replay.LicenseeVersion >= 34)
			{
				bUnconstrainRotation = br.ReadBit();
				bFreeLookSmoothing = br.ReadBit();
				cameraAccelRate = br.ReadSingle();
				cameraDecelRate = br.ReadSingle();
				freeLookSpeed = br.ReadSingle();
			}

			return new ProfileCameraSettings(fov, height, pitch, distance, stiffness, swivelSpeed, transitionSpeed, bUnconstrainRotation, bFreeLookSmoothing, cameraAccelRate, cameraDecelRate, freeLookSpeed);
		}

		public readonly void Serialize(BitWriter bw, Replay replay)
		{
			bw.Write(FOV);
			bw.Write(Height);
			bw.Write(Pitch);
			bw.Write(Distance);
			bw.Write(Stiffness);
			bw.Write(SwivelSpeed);

			if (replay.LicenseeVersion >= 20) bw.Write(TransitionSpeed);

			if(replay.LicenseeVersion >= 34)
			{
				bw.Write(bUnconstrainRotation);
				bw.Write(bFreeLookSmoothing);
				bw.Write(CameraAccelRate);
				bw.Write(CameraDecelRate);
				bw.Write(FreeLookSpeed);
			}
		}
	}
}
