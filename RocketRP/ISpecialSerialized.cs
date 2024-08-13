using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public interface ISpecialSerialized
	{
		void Deserialize(BinaryReader br, IFileVersionInfo versionInfo);

		void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo);
	}
}
