using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
