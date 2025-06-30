using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class BitWriter
	{
		private static readonly byte[] GShift = [0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80];
		private static readonly byte[] GMask = [0x00, 0x01, 0x03, 0x07, 0x0f, 0x1f, 0x3f, 0x7f];

		private byte[] Buffer;
		private int Pos;
		private int Max;

		public int NumBytes => (Pos + 7) >> 3;
		public int NumBits => Pos;
		public int MaxBits => Max;

		public BitWriter(int maxBits = 1024)
		{
			Buffer = new byte[(maxBits + 7) >> 3];
			Pos = 0;
			Max = maxBits;
		}

		private unsafe void Grow(int minBits)
		{
			while (minBits >= Max)
			{
				if (Max <= 0) Max = 1024;
				else Max *= 2;
			}

			var newBuffer = new byte[(Max + 7) >> 3];
			System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Buffer.Length);
			Buffer = newBuffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void SerializeBits(in void* src, int lengthBits)
		{
			if (Pos + lengthBits >= Max) Grow(Pos + lengthBits);

			//if (lengthBits == 1)
			//{
			//	if ((((byte*)src)[0] & 1) > 0)
			//	{
			//		Buffer[Pos >> 3] |= GShift[Pos & 7];
			//	}
			//	Pos++;
			//}
			//else
			//{
				//if (Pos + lengthBits >= Max) throw new OverflowException("Buffer overflow");
				fixed (byte* addr = &Buffer[0])
					BitArray.BitsCopy(addr, Pos, (byte*)src, 0, lengthBits);
				Pos += lengthBits;
			//}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SerializeInt(UInt32 value, UInt32 valueMax)
		{
			var maxLengthBits = BitOperations.Log2(valueMax) + 1;
			if (Pos + maxLengthBits >= Max) Grow(Pos + maxLengthBits);

			value = Math.Min(value, valueMax);
			Int32 newValue = 0;
			for (Int32 mask = 1; (newValue | mask) < valueMax && mask > 0; mask <<= 1, Pos++)
			{
				if ((value & mask) > 0)
				{
					Buffer[Pos >> 3] |= GShift[Pos & 7];
					newValue |= mask;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write([NotNull] Boolean? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			if (Pos + 1 >= Max) Grow(Pos + 1);

			if ((Boolean)value) Buffer[Pos >> 3] |= GShift[Pos & 7];
			Pos++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] Byte? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (Byte)value;
			SerializeBits(&val, sizeof(Byte) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write([NotNull] Int32? value, UInt32 valueMax)
		{
			ArgumentNullException.ThrowIfNull(value);
			SerializeInt((UInt32)value, valueMax);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write([NotNull] UInt32? value, UInt32 valueMax)
		{
			ArgumentNullException.ThrowIfNull(value);
			SerializeInt((UInt32)value, valueMax);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] Int32? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (Int32)value;
			SerializeBits(&val, sizeof(Int32) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] UInt32? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (UInt32)value;
			SerializeBits(&val, sizeof(UInt32) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] Int64? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (Int64)value;
			SerializeBits(&val, sizeof(Int64) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] UInt64? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (UInt64)value;
			SerializeBits(&val, sizeof(UInt64) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write([NotNull] Single? value)
		{
			ArgumentNullException.ThrowIfNull(value);
			var val = (Single)value;
			SerializeBits(&val, sizeof(Single) << 3);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Write(Byte[] bytes)
		{
			if (bytes.Length <= 0) return;
			fixed (byte* addr = &bytes[0])
				SerializeBits(addr, (sizeof(Byte) * bytes.Length) << 3);
		}

		public unsafe void Write(string? value)
		{
			if (value is null)
			{
				Write((Int32)0);
				return;
			}

			Int32 length = value.Length;
			if (value.Any(c => c > 255))
			{
				Write(-length - 1);
				length *= 2;
				fixed (void* addr = &value.GetPinnableReference())
					SerializeBits(addr, length << 3);
				Write([(Byte)0, (Byte)0]);
			}
			else
			{
				Write(length + 1);
				var bytes = CodePagesEncodingProvider.Instance.GetEncoding(1252)?.GetBytes(value) ?? throw new Exception("Code page 1252 is not available");
				fixed (byte* addr = &bytes[0])
					SerializeBits(addr, length << 3);
				Write((Byte)0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte[] GetAllBytes()
		{
			var data = new byte[(Pos + 7) >> 3];
			System.Buffer.BlockCopy(Buffer, 0, data, 0, data.Length);
			return data;
		}
	}
}
