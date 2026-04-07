using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Vector : ISpecialSerialized
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector operator *(Vector vector, float scalar)
		{
			vector.X *= scalar;
			vector.Y *= scalar;
			vector.Z *= scalar;
			return vector;
		}

		public static Vector operator /(Vector vector, float scalar)
		{
			vector.X /= scalar;
			vector.Y /= scalar;
			vector.Z /= scalar;
			return vector;
		}

		public void Deserialize(BinaryReader br, IFileVersionInfo versionInfo)
		{
			X = br.ReadSingle();
			Y = br.ReadSingle();
			Z = br.ReadSingle();
		}

		public void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			bw.Write(X);
			bw.Write(Y);
			bw.Write(Z);
		}

		public static Vector Deserialize(BitReader br, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22U : 20U;

			var numBits = br.ReadInt32(maxValuePerComponent);

			var bias = 1 << (numBits + 1);
			var max = 1U << (numBits + 2);

			var x = br.ReadInt32(max) - bias;
			var y = br.ReadInt32(max) - bias;
			var z = br.ReadInt32(max) - bias;

			return new Vector(x, y, z);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22U : 20U;

			var x = (int)MathF.Round(X);
			var y = (int)MathF.Round(Y);
			var z = (int)MathF.Round(Z);

			var maxValue = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));
			var numBitsForValue = (uint)MathF.Ceiling(MathF.Log2(maxValue + 1));
			var numBits = (int)Math.Clamp(numBitsForValue, 1, maxValuePerComponent) - 1;

			bw.Write(numBits, maxValuePerComponent);

			var bias = 1 << (numBits + 1);
			var max = 1U << (numBits + 2);
			var dx = (uint)(x + bias);
			var dy = (uint)(y + bias);
			var dz = (uint)(z + bias);

			bw.Write(dx, max);
			bw.Write(dy, max);
			bw.Write(dz, max);
		}
	}
}
