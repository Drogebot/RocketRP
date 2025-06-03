using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public static class BitArray
	{
		public static unsafe void BitsCopy(byte* dest, int destBit, in byte* src, int srcBit, int bitCount)
		{
			if (bitCount == 0) return;

			if (bitCount <= 8)
			{
				int destIndex = destBit >> 3;
				int srcIndex = srcBit >> 3;
				int lastDest = (destBit + bitCount - 1) >> 3;
				int lastSrc = (srcBit + bitCount - 1) >> 3;
				int shiftDest = destBit & 7;
				int shiftSrc = srcBit & 7;
				int firstMask = 0xFF << shiftDest;
				int lastMask = 0xFE << ((destBit + bitCount - 1) & 7);
				int accu;

				if (srcIndex == lastSrc)
					accu = src[srcIndex] >> shiftSrc;
				else
					accu = src[srcIndex] >> shiftSrc | (src[lastSrc] << (8 - shiftSrc));

				if (destIndex == lastDest)
				{
					int multiMask = firstMask & ~lastMask;
					dest[destIndex] = (byte)((dest[destIndex] & ~multiMask) | ((accu << shiftDest) & multiMask));
				}
				else
				{
					dest[destIndex] = (byte)((dest[destIndex] & ~firstMask) | ((accu << shiftDest) & firstMask));
					dest[lastDest] = (byte)((dest[lastDest] & lastMask) | ((accu >> (8 - shiftDest)) & ~lastMask));
				}
				return;
			}
			else
			{
				int destIndex = destBit >> 3;
				int firstSrcMask = 0xFF << (destBit & 7);
				int lastDest = (destBit + bitCount) >> 3;
				int lastSrcMask = 0xFF << ((destBit + bitCount) & 7);
				int srcIndex = srcBit >> 3;
				int lastSrc = (srcBit + bitCount) >> 3;
				int shiftCount = (destBit & 7) - (srcBit & 7);
				int destLoop = lastDest - destIndex;
				int srcLoop = lastSrc - srcIndex;
				int fullLoop, bitAccu;

				if (shiftCount >= 0)
				{
					fullLoop = int.Max(destLoop, srcLoop);
					bitAccu = src[srcIndex] << shiftCount;
					shiftCount += 8;
				}
				else
				{
					shiftCount += 8;
					fullLoop = int.Max(destLoop, srcLoop - 1);
					bitAccu = src[srcIndex] << shiftCount;
					srcIndex++;
					shiftCount += 8;
					bitAccu = ((src[srcIndex] << shiftCount) + bitAccu) >> 8;
				}

				dest[destIndex] = (byte)((bitAccu & firstSrcMask) | (dest[destIndex] & ~firstSrcMask));
				srcIndex++;
				destIndex++;

				for (; fullLoop > 1; fullLoop--)
				{
					bitAccu = ((src[srcIndex] << shiftCount) + bitAccu) >> 8;
					srcIndex++;
					dest[destIndex] = (byte)bitAccu;
					destIndex++;
				}

				if (lastSrcMask != 0xFF)
				{
					if ((srcBit + bitCount - 1) >> 3 == srcIndex)
					{
						bitAccu = ((src[srcIndex] << shiftCount) + bitAccu) >> 8;
					}
					else
					{
						bitAccu = bitAccu >> 8;
					}

					dest[destIndex] = (byte)((dest[destIndex] & lastSrcMask) | (bitAccu & ~lastSrcMask));
				}
			}
		}
	}
}
