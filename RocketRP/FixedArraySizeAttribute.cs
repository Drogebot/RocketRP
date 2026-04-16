using System;

namespace RocketRP
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	internal class FixedArraySize : Attribute
	{
		public int Size;

		public FixedArraySize(int size)
		{
			Size = size;
		}
	}
}
