using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Core
{
	public class ClassObject : Core.Object
	{
		public string Name { get; set; }

		public ClassObject(string name)
		{
			Name = name;
		}
	}
}
