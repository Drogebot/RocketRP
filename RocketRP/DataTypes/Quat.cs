using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	/// This code was adapted from https://www.reddit.com/r/RocketLeague/comments/93h1f3/psyonix_help_me_please_how_are_rotations_encoded/e3g5b2s/
	public struct Quat
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		private const int NUM_BITS = 18;
		private const int MAX_VALUE = (1 << NUM_BITS) - 1;
		private const float MAX_QUAT_VALUE = 0.7071067811865475244f;    // 1/sqrt(2)
		private const float INV_MAX_QUAT_VALUE = 1.0f / MAX_QUAT_VALUE;

		private enum Component : byte
		{
			X,
			Y,
			Z,
			W,
			Num
		}

		public Quat(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public static Quat operator /(Quat quat, float scalar)
		{
			quat.X /= scalar;
			quat.Y /= scalar;
			quat.Z /= scalar;
			quat.W /= scalar;
			return quat;
		}

		public void Normalize()
		{
			float mag = MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
			this /= mag;
		}

		private static float UncompressComponent(uint iValue)
		{
			float positiveRangedValue = iValue / (float)MAX_VALUE;
			float rangedValue = (positiveRangedValue - 0.50f) * 2.0f;
			return rangedValue * MAX_QUAT_VALUE;
		}

		private static void UncompressComponents(BitReader br, ref float a, ref float b, ref float c, ref float missing)
		{
			a = UncompressComponent(br.ReadUInt32(MAX_VALUE + 1));
			b = UncompressComponent(br.ReadUInt32(MAX_VALUE + 1));
			c = UncompressComponent(br.ReadUInt32(MAX_VALUE + 1));
			missing = MathF.Sqrt(1.0f - (a * a) - (b * b) - (c * c));
		}

		public static Quat Deserialize(BitReader br)
		{
			var largestComponent = (Component)br.ReadInt32((byte)Component.Num);

			var quat = new Quat();

			switch (largestComponent)
			{
				case Component.X:
					UncompressComponents(br, ref quat.Y, ref quat.Z, ref quat.W, ref quat.X);
					break;
				case Component.Y:
					UncompressComponents(br, ref quat.X, ref quat.Z, ref quat.W, ref quat.Y);
					break;
				case Component.Z:
					UncompressComponents(br, ref quat.X, ref quat.Y, ref quat.W, ref quat.Z);
					break;
				case Component.W:
					UncompressComponents(br, ref quat.X, ref quat.Y, ref quat.Z, ref quat.W);
					break;
			}

			quat.Normalize();

			return quat;
		}

		private static uint CompressComponent(float value)
		{
			float rangedValue = value * INV_MAX_QUAT_VALUE;
			float positiveRangedValue = (rangedValue / 2.0f) + 0.50f;
			return (uint)MathF.Round(positiveRangedValue * MAX_VALUE);
		}

		private void CompressComponents(BitWriter bw, ref float a, ref float b, ref float c)
		{
			bw.Write(CompressComponent(a), MAX_VALUE + 1);
			bw.Write(CompressComponent(b), MAX_VALUE + 1);
			bw.Write(CompressComponent(c), MAX_VALUE + 1);
		}

		public void Serialize(BitWriter bw)
		{
			Normalize();

			var components = new List<float> { X, Y, Z, W };
			var largestComponent = Component.X;
			var largestComponentAbsValue = 0f;
			for (var i = 0; i < components.Count; i++)
			{
				var componentAbs = Math.Abs(components[i]);
				if (componentAbs > largestComponentAbsValue)
				{
					largestComponent = (Component)i;
					largestComponentAbsValue = componentAbs;
				}
			}

			var largestComponentValue = components[(byte)largestComponent];
			if (largestComponentValue < 0) this /= -1;

			bw.Write((byte)largestComponent, (byte)Component.Num);

			switch (largestComponent)
			{
				case Component.X:
					CompressComponents(bw, ref Y, ref Z, ref W);
					break;
				case Component.Y:
					CompressComponents(bw, ref X, ref Z, ref W);
					break;
				case Component.Z:
					CompressComponents(bw, ref X, ref Y, ref W);
					break;
				case Component.W:
					CompressComponents(bw, ref X, ref Y, ref Z);
					break;
			}
		}
	}
}