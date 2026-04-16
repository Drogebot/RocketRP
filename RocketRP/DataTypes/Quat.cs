using System;
using System.Collections.Generic;

namespace RocketRP.DataTypes
{
	/// This code was adapted from https://www.reddit.com/r/RocketLeague/comments/93h1f3/psyonix_help_me_please_how_are_rotations_encoded/e3g5b2s/
	public struct Quat
	{
		private float _x;
		private float _y;
		private float _z;
		private float _w;
		public float X { readonly get => _x; set => _x = value; }
		public float Y { readonly get => _y; set => _y = value; }
		public float Z { readonly get => _z; set => _z = value; }
		public float W { readonly get => _w; set => _w = value; }

		private const int NUM_BITS = 18;
		private const int MAX_VALUE = (1 << NUM_BITS) - 1;
		private const float MAX_QUAT_VALUE = 0.7071067811865475244f;	// 1/sqrt(2)
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

		public Quat(Rotator rotator)
		{
			var cr = MathF.Cos(rotator.Roll * (MathF.PI / 180) / 2);
			var cp = MathF.Cos(rotator.Pitch * (MathF.PI / 180) / 2);
			var cy = MathF.Cos(rotator.Yaw * (MathF.PI / 180) / 2);
			var sr = MathF.Sin(rotator.Roll * (MathF.PI / 180) / 2);
			var sp = MathF.Sin(rotator.Pitch * (MathF.PI / 180) / 2);
			var sy = MathF.Sin(rotator.Yaw * (MathF.PI / 180) / 2);

			W = cr * cp * cy + sr * sp * sy;
			X = sr * cp * cy - cr * sp * sy;
			Y = cr * sp * cy + sr * cp * sy;
			Z = cr * cp * sy - sr * sp * cy;
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
					UncompressComponents(br, ref quat._y, ref quat._z, ref quat._w, ref quat._x);
					break;
				case Component.Y:
					UncompressComponents(br, ref quat._x, ref quat._z, ref quat._w, ref quat._y);
					break;
				case Component.Z:
					UncompressComponents(br, ref quat._x, ref quat._y, ref quat._w, ref quat._z);
					break;
				case Component.W:
					UncompressComponents(br, ref quat._x, ref quat._y, ref quat._z, ref quat._w);
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
					CompressComponents(bw, ref _y, ref _z, ref _w);
					break;
				case Component.Y:
					CompressComponents(bw, ref _x, ref _z, ref _w);
					break;
				case Component.Z:
					CompressComponents(bw, ref _x, ref _y, ref _w);
					break;
				case Component.W:
					CompressComponents(bw, ref _x, ref _y, ref _z);
					break;
			}
		}
	}
}