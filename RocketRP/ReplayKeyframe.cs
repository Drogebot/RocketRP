using System.Text.Json.Serialization;

namespace RocketRP
{
	public struct ReplayKeyframe
	{
		public float Time { get; set; }
		public int frame { get; set; }
		[JsonIgnore]
		public int Position { get; set; }

		public static ReplayKeyframe Deserialize(BinaryReader br)
		{
			var keyFrame = new ReplayKeyframe
			{
				Time = br.ReadSingle(),
				frame = br.ReadInt32(),
				Position = br.ReadInt32()
			};

			return keyFrame;
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			bw.Write(Time);
			bw.Write(frame);
			bw.Write(Position);
		}
	}
}
