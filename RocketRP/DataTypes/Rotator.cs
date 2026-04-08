using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Rotator : ISpecialSerialized
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

		public Rotator(Quat quat)
		{
			var sinp = 2 * (quat.W * quat.Y - quat.Z * quat.X);
			
			if (MathF.Abs(sinp) >= 1f - 1e-4f)
			{
				Pitch = MathF.CopySign(MathF.PI / 2, sinp);
				Yaw = MathF.Atan2(-2 * (quat.W * quat.Z - quat.X * quat.Y), 1 - 2 * (quat.X * quat.X + quat.Z * quat.Z));
				Roll = 0;
			}
			else
			{
				Pitch = MathF.Asin(sinp);
				Yaw = MathF.Atan2(2 * (quat.W * quat.Z + quat.X * quat.Y), 1 - 2 * (quat.Y * quat.Y + quat.Z * quat.Z));
				Roll = MathF.Atan2(2 * (quat.W * quat.X + quat.Y * quat.Z), 1 - 2 * (quat.X * quat.X + quat.Y * quat.Y));
			}

			Pitch *= 180 / MathF.PI;
			Yaw *= 180 / MathF.PI;
			Roll *= 180 / MathF.PI;
		}

		public void Deserialize(BinaryReader br, IFileVersionInfo versionInfo)
		{
			Pitch = br.ReadSingle();
			Yaw = br.ReadSingle();
			Roll = br.ReadSingle();
		}

		public void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			bw.Write(Pitch);
			bw.Write(Yaw);
			bw.Write(Roll);
		}

		private static float ByteToAxis(byte b)
		{
			return b * (180f / 128f);
		}

		public static Rotator Deserialize(BitReader br, Replay replay)
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
			return (byte)(Normalize(axis) * (128f / 180f));
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			byte b = AxisToByte(Pitch);
			if (bw.Write(b != 0))
				bw.Write(b);
			b = AxisToByte(Yaw);
			if (bw.Write(b != 0))
				bw.Write(b);
			b = AxisToByte(Roll);
			if (bw.Write(b != 0))
				bw.Write(b);
		}

		public static Rotator DeserializeUncompressed(BitReader br, Replay replay)
		{
			var pitch = br.ReadInt32(1U << 16) * (180f / 32768f);
			var yaw = br.ReadInt32(1U << 16) * (180f / 32768f);
			var roll = br.ReadInt32(1U << 16) * (180f / 32768f);

			return new Rotator(pitch, yaw, roll);
		}

		public void SerializeUncompressed(BitWriter bw, Replay replay)
		{
			bw.Write((int)(Pitch * (32768f / 180f)), 1U << 16);
			bw.Write((int)(Yaw * (32768f / 180f)), 1U << 16);
			bw.Write((int)(Roll * (32768f / 180f)), 1U << 16);
		}
	}
}
