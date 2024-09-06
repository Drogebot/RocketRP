using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public interface IArrayProperty
	{
		object GetValues();
		void Deserialize(BitReader br, Replay replay);
		void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo);
		void Serialize(BitWriter bw, Replay replay, int propId, int maxPropertyId);
	}

	public struct ArrayProperty<T> : IArrayProperty
	{
		public T[] Values { get; set; }
		[JsonIgnore]
		public int Length => Values.Length;

		public ArrayProperty(int capacity)
		{
			Values = new T[capacity];
		}

		public ArrayProperty(T[] values)
		{
			Values = values;
		}

		public ArrayProperty(List<T> values)
		{
			Values = values.ToArray();
		}

		public T this[int index]
		{
			get => Values[index];
			set => Values[index] = value;
		}

		public object GetValues() => Values;

		public static ArrayProperty<T> Deserialize(BinaryReader br, IFileVersionInfo versionInfo)
		{
			var array = new ArrayProperty<T>(br.ReadInt32());
			for (int i = 0; i < array.Length; i++)
			{
				if (typeof(T) == typeof(int)) array[i] = (T)(object)br.ReadInt32();
				else if (typeof(T) == typeof(uint)) array[i] = (T)(object)br.ReadUInt32();
				else if (typeof(T) == typeof(long)) array[i] = (T)(object)br.ReadInt64();
				else if (typeof(T) == typeof(ulong)) array[i] = (T)(object)br.ReadUInt64();
				else if (typeof(T) == typeof(float)) array[i] = (T)(object)br.ReadSingle();
				else if (typeof(T) == typeof(string)) array[i] = (T)(object)"".Deserialize(br);
				else if (typeof(T) == typeof(Name)) array[i] = (T)(object)Name.Deserialize(br);
				else if (typeof(T) == typeof(byte)) array[i] = (T)(object)br.ReadByte();
				else if (typeof(T).IsEnum) array[i] = (T)Enum.Parse(typeof(T), "".Deserialize(br));
				else if (typeof(T) == typeof(ObjectTarget)) array[i] = (T)(object)ObjectTarget.Deserialize(br);
				else
				{
					var propertyType = typeof(T);
					var value = Activator.CreateInstance(propertyType);
					if(value is ISpecialSerialized specialValue) specialValue.Deserialize(br, versionInfo);
					else Actors.Core.Object.Deserialize(value, br, versionInfo);
					array[i] = (T)value;
				}
			}

			return array;
		}

		public void Deserialize(BitReader br, Replay replay)
		{
			var index = br.ReadInt32Max(Length);
			T value;

			if (typeof(T) == typeof(byte)) value = (T)(object)br.ReadByte();
			else if (typeof(T) == typeof(int)) value = (T)(object)br.ReadInt32();
			else if (typeof(T) == typeof(uint)) value = (T)(object)br.ReadUInt32();
			else if (typeof(T) == typeof(long)) value = (T)(object)br.ReadInt64();
			else if (typeof(T) == typeof(ulong)) value = (T)(object)br.ReadUInt64();
			else if (typeof(T) == typeof(float)) value = (T)(object)br.ReadSingle();
			else if (typeof(T) == typeof(string)) value = (T)(object)br.ReadString();
			else
			{
				var methodInfo = typeof(T).GetMethod("Deserialize", new Type[] { typeof(BitReader) }) ?? typeof(T).GetMethod("Deserialize", new Type[] { typeof(BitReader), typeof(Replay) });
				if (methodInfo.GetParameters().Length == 1) value = (T)methodInfo.Invoke(null, new object[] { br });
				else if (methodInfo.GetParameters().Length == 2) value = (T)methodInfo.Invoke(null, new object[] { br, replay });
				else throw new MethodAccessException($"Deserialize method in {typeof(T).Name} must have 1 or 2 parameters");
			}

			Values[index] = value;
		}

		public void Serialize(BinaryWriter bw, IFileVersionInfo versionInfo)
		{
			bw.Write(Length);

			foreach (var item in Values)
			{
				if (item is bool boolvalue) bw.Write(boolvalue);
				else if (item is int intvalue) bw.Write(intvalue);
				else if (item is uint uintvalue) bw.Write(uintvalue);
				else if (item is long longvalue) bw.Write(longvalue);
				else if (item is ulong ulongvalue) bw.Write(ulongvalue);
				else if (item is float floatvalue) bw.Write(floatvalue);
				else if (item is string stringvalue) stringvalue.Serialize(bw);
				else if (item is Name namevalue) namevalue.Serialize(bw);
				else if (item is byte bytevalue) bw.Write(bytevalue);
				else if (item is Enum enumvalue) enumvalue.ToString().Serialize(bw);
				else if (item is ObjectTarget objectvalue) objectvalue.Serialize(bw);
				else
				{
					if(item is ISpecialSerialized specialItem) specialItem.Serialize(bw, versionInfo);
					else Actors.Core.Object.Serialize(item, bw, versionInfo);
				}
			}
		}

		public void Serialize(BitWriter bw, Replay replay, int propId, int maxPropertyId)
		{
			bool firstEntry = true;
			for (int i = 0; i < Length; i++)
			{
				if (Values[i].Equals(default(T))) continue;
				if (!firstEntry)
				{
					bw.Write(true);
					bw.Write(propId, maxPropertyId);
				}
				bw.Write(i, Length);

				var methodInfo = typeof(T).GetMethod("Serialize", new Type[] { typeof(BitWriter) }) ?? typeof(T).GetMethod("Serialize", new Type[] { typeof(BitWriter), typeof(Replay) });
				if (methodInfo.GetParameters().Length == 1) methodInfo.Invoke(Values[i], new object[] { bw });
				else if (methodInfo.GetParameters().Length == 2) methodInfo.Invoke(Values[i], new object[] { bw, replay });
				else throw new MethodAccessException($"Serialize method in {typeof(T).Name} must have 1 or 2 parameters");

				firstEntry = false;
			}
		}
	}
}
