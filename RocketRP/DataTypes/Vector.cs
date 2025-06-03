using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Vector
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

		public static Vector operator *(Vector vector, int scalar)
		{
			vector.X *= scalar;
			vector.Y *= scalar;
			vector.Z *= scalar;
			return vector;
		}

		public static Vector operator /(Vector vector, int scalar)
		{
			vector.X /= scalar;
			vector.Y /= scalar;
			vector.Z /= scalar;
			return vector;
		}

		public static Vector Deserialize(BitReader br, Replay replay)
		{
			var maxValuePerComponent = replay.NetVersion >= 7 ? 22U : 20U;

			var numBits = br.ReadInt32(maxValuePerComponent);
			var bias = 1 << (numBits + 1);
			var maxBits = numBits + 2;

			var x = br.ReadInt32(1U << maxBits) - bias;
			var y = br.ReadInt32(1U << maxBits) - bias;
			var z = br.ReadInt32(1U << maxBits) - bias;

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
			var bias = 1 << (numBits + 1);
			var maxBits = numBits + 2;
			var max = 1 << (numBits + 2);

			bw.Write(numBits, maxValuePerComponent);

			var dx = x + bias;
			var dy = y + bias;
			var dz = z + bias;

			if (dx >= max) dx = max - 1;
			if (dy >= max) dy = max - 1;
			if (dz >= max) dz = max - 1;

			bw.Write(dx, 1U << maxBits);
			bw.Write(dy, 1U << maxBits);
			bw.Write(dz, 1U << maxBits);
		}

		public static Vector DeserializeFixedPoint(BitReader br, Replay replay)
		{
			var vector = Deserialize(br, replay);
			
			vector /= 100;

			return vector;
		}

		public void SerializeFixedPoint(BitWriter bw, Replay replay)
		{
			this *= 100;

			Serialize(bw, replay);
		}

		public static Vector DeserializeFixed(BitReader br, int numBits)
		{
			var maxBitValue = (1 << (numBits - 1)) - 1;
			var bias = 1 << (numBits - 1);

			var x = br.ReadInt32(1U << numBits) - bias;
			var y = br.ReadInt32(1U << numBits) - bias;
			var z = br.ReadInt32(1U << numBits) - bias;

			return new Vector(x, y, z) / maxBitValue;
		}

		public void SerializeFixed(BitWriter bw, int numBits)
		{
			var maxBitValue = (1 << (numBits - 1)) - 1;
			var bias = 1 << (numBits - 1);

			this *= maxBitValue;

			bw.Write((int)X + bias, 1U << numBits);
			bw.Write((int)Y + bias, 1U << numBits);
			bw.Write((int)Z + bias, 1U << numBits);
		}
	}
}
