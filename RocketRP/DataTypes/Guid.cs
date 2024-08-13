using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct Guid : ISpecialSerialized
	{
		public int A { get; set; }
		public int B { get; set; }
		public int C { get; set; }
		public int D { get; set; }

		public void Deserialize(BinaryReader br, IFileVersionInfo versionInfo)
		{
			A = br.ReadInt32();
			B = br.ReadInt32();
			C = br.ReadInt32();
			D = br.ReadInt32();
		}

		public void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			bw.Write(A);
			bw.Write(B);
			bw.Write(C);
			bw.Write(D);
		}
	}
}
