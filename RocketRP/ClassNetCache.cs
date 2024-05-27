using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class ClassNetCache
	{
		public int ObjectIndex { get; set; }
		public int MinPropertyId { get; set; }
		public int MaxPropertyId { get; set; }
		public List<ClassNetCacheProperty> Properties { get; set; }
		public int NumProperties => Properties.Count > 0 ? Properties.Max(p => p.PropertyId) + 1 : Parent != null ? Parent.NumProperties : 0;
		public ClassNetCache Parent;

		public int GetPropertyObjectIndex(int propId)
		{
			foreach (var property in Properties)
			{
				if (property.PropertyId == propId)
				{
					return property.ObjectIndex;
				}
			}

			return Parent.GetPropertyObjectIndex(propId);
		}

		public int GetPropertyPropertyId(int objectIndex)
		{
			foreach (var property in Properties)
			{
				if (property.ObjectIndex == objectIndex)
				{
					return property.PropertyId;
				}
			}

			return Parent.GetPropertyPropertyId(objectIndex);
		}

		public void CalculateParent(Replay replay)
		{
			var type = System.Type.GetType($"RocketRP.Actors.{replay.Objects[ObjectIndex]}");
			if (type.BaseType != typeof(object))
			{
				var baseObjectIndex = replay.Objects.IndexOf(type.BaseType.FullName.Replace("RocketRP.Actors.", ""));
				Parent = replay.ClassNetCaches.First(c => c.ObjectIndex == baseObjectIndex);
			}
		}

		public static ClassNetCache Deserialize(BinaryReader br)
		{
			var classNetCache = new ClassNetCache();

			classNetCache.ObjectIndex = br.ReadInt32();
			classNetCache.MinPropertyId = br.ReadInt32();
			classNetCache.MaxPropertyId = br.ReadInt32();

			var numProperties = br.ReadInt32();
			classNetCache.Properties = new List<ClassNetCacheProperty>(numProperties);
			for (int i = 0; i < numProperties; i++)
			{
				classNetCache.Properties.Add(ClassNetCacheProperty.Deserialize(br));
			}

			return classNetCache;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(ObjectIndex);
			bw.Write(MinPropertyId);
			bw.Write(MaxPropertyId);
			bw.Write(Properties.Count);
			foreach (var property in Properties)
			{
				property.Serialize(bw);
			}
		}
	}

	public class ClassNetCacheProperty
	{
		public int ObjectIndex { get; set; }
		public int PropertyId { get; set; }

		public static ClassNetCacheProperty Deserialize(BinaryReader br)
		{
			var classNetCacheProperty = new ClassNetCacheProperty();

			classNetCacheProperty.ObjectIndex = br.ReadInt32();
			classNetCacheProperty.PropertyId = br.ReadInt32();

			return classNetCacheProperty;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(ObjectIndex);
			bw.Write(PropertyId);
		}
	}
}
