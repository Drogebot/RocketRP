using RocketRP.DataTypes;

namespace RocketRP
{
	public struct TimelineKeyframe
	{
		public Name Type { get; set; }
		public int frame { get; set; }

		public static TimelineKeyframe Deserialize(BinaryReader br)
		{
			var tickmark = new TimelineKeyframe
			{
				Type = Name.Deserialize(br),
				frame = br.ReadInt32()
			};

			return tickmark;
		}

		public readonly void Serialize(BinaryWriter bw)
		{
			Type.Serialize(bw);
			bw.Write(frame);
		}
	}
}
