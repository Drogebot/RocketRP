using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public interface IFileVersionInfo
	{
		public uint EngineVersion { get; set; }
		public uint LicenseeVersion { get; set; }
		public uint TypeVersion { get; set; }
	}
}
