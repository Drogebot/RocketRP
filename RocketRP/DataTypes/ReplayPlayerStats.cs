using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public struct ReplayPlayerStats
	{
		public StrProperty? Name { get; set; }

		public ByteProperty? Platform { get; set; }

		public QWordProperty? OnlineID { get; set; }

		public IntProperty? Team { get; set; }

		public IntProperty? Score { get; set; }

		public IntProperty? Goals { get; set; }

		public IntProperty? Assists { get; set; }

		public IntProperty? Saves { get; set; }

		public IntProperty? Shots { get; set; }

		public BoolProperty bBot { get; set; }
	}
}
