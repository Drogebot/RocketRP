using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.DataTypes
{
	public interface IArrayProperty
	{
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

		public T this[int index]
		{
			get => Values[index];
			set => Values[index] = value;
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
			else value = (T)typeof(T).GetMethod("Deserialize").Invoke(null, [br, replay]);

			Values[index] = value;
		}

		public void Serialize(BitWriter bw, Replay replay, int propId, int maxPropertyId)
		{
			for (int i = 0; i < Length; i++)
			{
				if(i > 0)
				{
					bw.Write(true);
					bw.Write(propId, maxPropertyId);
				}
				bw.Write(i, Length);
				typeof(T).GetMethod("Serialize").Invoke(Values[i], [bw, replay]);
			}
		}
	}
}
