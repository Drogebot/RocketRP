using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Rotator
	{
		public float Pitch { get; set; }
		public float Yaw { get; set; }
		public float Roll { get; set; }

		public Rotator(float pitch, float yaw, float roll)
		{
			Pitch = pitch;
			Yaw = yaw;
			Roll = roll;
		}

		private static float ByteToAxis(byte b)
		{
			return b / 256f * 360f;
		}

		public static Rotator Deserialize(BitReader br)
		{
			float pitch = 0, yaw = 0, roll = 0;
			if (br.ReadBit())
				pitch = ByteToAxis(br.ReadByte());
			if (br.ReadBit())
				yaw = ByteToAxis(br.ReadByte());
			if (br.ReadBit())
				roll = ByteToAxis(br.ReadByte());

			return new Rotator(pitch, yaw, roll);
		}

		private static float Normalize(float axis)
		{
			axis %= 360f;
			if (axis < 0) axis += 360;
			return axis;
		}

		private static byte AxisToByte(float axis)
		{
			return (byte)(Normalize(axis) / 360f * 256f);
		}

		public void Serialize(BitWriter bw)
		{
			byte b = AxisToByte(Pitch);
			bw.Write(b != 0);
			if (b != 0)
			{
				bw.Write(b);
			}

			b = AxisToByte(Yaw);
			bw.Write(b != 0);
			if (b != 0)
			{
				bw.Write(b);
			}

			b = AxisToByte(Roll);
			bw.Write(b != 0);
			if (b != 0)
			{
				bw.Write(b);
			}
		}
	}
}
