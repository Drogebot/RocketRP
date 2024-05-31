using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public interface IArrayProperty
	{
		object GetValues();
		void Deserialize(BitReader br, Replay replay);
		void Serialize(BitWriter bw, Replay replay, int propId, int maxPropertyId);
	}

	public struct ArrayProperty<T> : IArrayProperty
	{
		public T[] Values { get; set; }
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

				var methodInfo = typeof(T).GetMethod("Deserialize");
				if (methodInfo.GetParameters().Length == 1) value = (T)methodInfo.Invoke(null, [br]);
				else if (methodInfo.GetParameters().Length == 2) value = (T)methodInfo.Invoke(null, [br, replay]);
				else throw new MethodAccessException($"Deserialize method in {typeof(T).Name} must have 1 or 2 parameters");
			}

			Values[index] = value;
		}

		public void Serialize(BitWriter bw, Replay replay, int propId, int maxPropertyId)
		{
			bool firstEntry = true;
			for (int i = 0; i < Length; i++)
			{
				if (Values[i].Equals(default(T))) continue;
				if(!firstEntry)
				{
					bw.Write(true);
					bw.Write(propId, maxPropertyId);
				}
				bw.Write(i, Length);
				typeof(T).GetMethod("Serialize").Invoke(Values[i], [bw, replay]);
				firstEntry = false;
			}
		}
	}
}
