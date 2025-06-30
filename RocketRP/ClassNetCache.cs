using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public class ClassNetCache
	{
		public int ObjectIndex { get; set; }
		public int MinPropertyId { get; set; }
		public int MaxPropertyId { get; set; }
		public List<ClassNetCacheProperty> Properties { get; set; } = null!;
		[JsonIgnore]
		public int NumProperties => Properties.Count + (Parent?.NumProperties ?? 0);
		[JsonIgnore]
		public int NumFields => MaxPropertyId - 1;
		[JsonIgnore]
		public int NumFuncs => NumProperties - MaxPropertyId;
		[JsonIgnore]
		public ClassNetCache? Parent;
		[JsonIgnore]
		public Type ClassType = null!;

		public ClassNetCacheProperty GetPropertyByPropertyId(int propId)
		{
			if(propId < MinPropertyId) return Parent?.GetPropertyByPropertyId(propId) ?? throw new NullReferenceException($"PropertyId {propId} is less than MinPropertyId {MinPropertyId} and no parent was specified");
			return Properties[propId - MinPropertyId];
		}

		public ClassNetCacheProperty? GetPropertyByName(string propName)
		{
			foreach (var property in Properties)
			{
				if (property.PropertyInfo?.Name == propName)
				{
					return property;
				}
			}

			return Parent?.GetPropertyByName(propName);
		}

		public void CalculateParent(Replay replay)
		{
			var type = System.Type.GetType($"RocketRP.Actors.{replay.Objects[ObjectIndex]}") ?? throw new NullReferenceException();
			var baseType = type;
			var baseObjectIndex = -1;
			while (baseObjectIndex == -1)
			{
				if (baseType.BaseType == typeof(object)) break;
				baseType = baseType.BaseType!;
				baseObjectIndex = replay.Objects.IndexOf(baseType.FullName!.Replace("RocketRP.Actors.", ""));
			}

			if (baseObjectIndex != -1) Parent = replay.ClassNetCaches.First(c => c.ObjectIndex == baseObjectIndex);
		}

		public bool LinkTypeAndPropertyInfos(List<string> objects)
		{
			if (ClassType is not null) return true; // Already linked
			var result = true;
			ClassType = System.Type.GetType($"RocketRP.Actors.{objects[ObjectIndex]}") ?? throw new NullReferenceException();
			for (int i = 0; i < Properties.Count; i++)
			{
				ClassNetCacheProperty? property = Properties[i];
				if (property.PropertyId < MinPropertyId) throw new ArgumentOutOfRangeException(nameof(property.PropertyId), $"PropertyId {property.PropertyId} is less than MinPropertyId {MinPropertyId}");
				if (property.PropertyId > NumProperties - 1) throw new ArgumentOutOfRangeException(nameof(property.PropertyId), $"PropertyId {property.PropertyId} is greater than NumProperties {NumProperties}");
				
				var propName = objects[property.ObjectIndex].Split(":").Last();
				if (property.PropertyId > MaxPropertyId - 1 + Parent?.NumFuncs)
				{
					//Console.WriteLine($"Skipping {property.PropertyId} ({objects[property.ObjectIndex].Split(":").Last()}) in {ClassType.Name} ({MinPropertyId}/{MaxPropertyId}/{NumProperties}, {NumFields}/{NumFuncs})");
					continue; // This is a function, those are never included in the data
				}
				var propertyInfo = ClassType.GetProperty(propName, BindingFlags.DeclaredOnly | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
				if (propertyInfo is null)
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine($"Warning: Property {property.PropertyId} ({property.ObjectIndex} {propName}) not found in {ClassType.Name}");
					Console.ForegroundColor = ConsoleColor.Gray;
					result = false;
					continue;
				}
				property.PropertyInfo = propertyInfo;
			}
			return result;
		}

		public static ClassNetCache Deserialize(BinaryReader br)
		{
			var classNetCache = new ClassNetCache
			{
				ObjectIndex = br.ReadInt32(),
				MinPropertyId = br.ReadInt32(),
				MaxPropertyId = br.ReadInt32(),
				Properties = new List<ClassNetCacheProperty>(br.ReadInt32())
			};
			for (int i = 0; i < classNetCache.Properties.Capacity; i++)
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
		[JsonIgnore]
		public PropertyInfo PropertyInfo = null!;

		public static ClassNetCacheProperty Deserialize(BinaryReader br)
		{
			var classNetCacheProperty = new ClassNetCacheProperty
			{
				ObjectIndex = br.ReadInt32(),
				PropertyId = br.ReadInt32()
			};

			return classNetCacheProperty;
		}

		public void Serialize(BinaryWriter bw)
		{
			bw.Write(ObjectIndex);
			bw.Write(PropertyId);
		}
	}
}
