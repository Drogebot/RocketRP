using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class ClassNetCache
	{
		public uint ObjectIndex { get; set; }
		public uint MinPropertyId { get; set; }
		public uint MaxPropertyId { get; set; }
		public List<ClassNetCacheProperty> Properties { get; set; }

		public static ClassNetCache Deserialize(BinaryReader br)
		{
			var classNetCache = new ClassNetCache();

			classNetCache.ObjectIndex = br.ReadUInt32();
			classNetCache.MinPropertyId = br.ReadUInt32();
			classNetCache.MaxPropertyId = br.ReadUInt32();

			var numProperties = br.ReadInt32();
			classNetCache.Properties = new List<ClassNetCacheProperty>(numProperties);
			for (int i = 0; i < numProperties; i++)
			{
				classNetCache.Properties.Add(ClassNetCacheProperty.Deserialize(br));
			}

			return classNetCache;
		}
	}

	public class ClassNetCacheProperty
	{
		public uint ObjectIndex { get; set; }
		public uint PropertyId { get; set; }

		public static ClassNetCacheProperty Deserialize(BinaryReader br)
		{
			var classNetCacheProperty = new ClassNetCacheProperty();

			classNetCacheProperty.ObjectIndex = br.ReadUInt32();
			classNetCacheProperty.PropertyId = br.ReadUInt32();

			return classNetCacheProperty;
		}
	}
}
