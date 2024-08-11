using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP
{
	public abstract class Property
	{
		public abstract object GetValue();
		public abstract void Serialize(BinaryWriter bw);
	}

	public class BoolProperty : Property
	{
		public bool Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static BoolProperty Deserialize(BinaryReader br)
		{
			var prop = new BoolProperty();
			prop.Value = br.ReadBoolean();

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator BoolProperty(bool value) => new BoolProperty { Value = value };
		public static implicit operator bool(BoolProperty prop) => prop.Value;
	}

	public class IntProperty : Property
	{
		public int Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static IntProperty Deserialize(BinaryReader br)
		{
			var prop = new IntProperty();
			prop.Value = br.ReadInt32();

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator IntProperty(int value) => new IntProperty { Value = value };
		public static implicit operator int(IntProperty prop) => prop.Value;
	}

	public class QWordProperty : Property
	{
		public ulong Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static QWordProperty Deserialize(BinaryReader br)
		{
			var prop = new QWordProperty();
			prop.Value = br.ReadUInt64();

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator QWordProperty(long value) => new QWordProperty { Value = (ulong)value };
		public static implicit operator QWordProperty(ulong value) => new QWordProperty { Value = value };
		public static implicit operator ulong(QWordProperty prop) => prop.Value;
	}

	public class FloatProperty : Property
	{
		public float Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static FloatProperty Deserialize(BinaryReader br)
		{
			var prop = new FloatProperty();
			prop.Value = br.ReadSingle();

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator FloatProperty(float value) => new FloatProperty { Value = value };
		public static implicit operator float(FloatProperty prop) => prop.Value;
	}

	public class StrProperty : Property
	{
		public string Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static StrProperty Deserialize(BinaryReader br)
		{
			var prop = new StrProperty();
			prop.Value = "".Deserialize(br);

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			Value.Serialize(bw);
		}

		public override string ToString()
		{
			return Value;
		}

		public static implicit operator StrProperty(string value) => new StrProperty { Value = value };
		public static implicit operator string(StrProperty prop) => prop?.Value ?? string.Empty;
	}

	public class NameProperty : Property
	{
		public string Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static NameProperty Deserialize(BinaryReader br)
		{
			var prop = new NameProperty();
			prop.Value = "".Deserialize(br);

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			Value.Serialize(bw);
		}

		public override string ToString()
		{
			return Value;
		}

		public static implicit operator NameProperty(string value) => new NameProperty { Value = value };
		public static implicit operator string(NameProperty prop) => prop?.Value ?? string.Empty;
	}

	public class ByteProperty : Property
	{
		public string EnumType { get; set; }
		public string Value { get; set; }

		public override object GetValue()
		{
			return ToString();
		}

		public static ByteProperty Deserialize(BinaryReader br)
		{
			var prop = new ByteProperty();
			prop.EnumType = "".Deserialize(br);
			prop.Value = "".Deserialize(br);

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			EnumType.Serialize(bw);
			Value.Serialize(bw);
		}

		public override string ToString()
		{
			return $"{EnumType}.{Value}";
		}

		public static implicit operator ByteProperty((string, string) value) => new ByteProperty { EnumType = value.Item1, Value = value.Item2 };
		public static implicit operator (string, string)(ByteProperty prop) => (prop.EnumType, prop.Value);
		public static implicit operator ByteProperty(string value) => new ByteProperty { EnumType = value.Split(".")[0], Value = value.Split(".")[1] };
		public static implicit operator string(ByteProperty prop) => prop.ToString();
		public static implicit operator ByteProperty(Enum value) => new ByteProperty { EnumType = value.GetType().Name, Value = value.ToString() };
		public static implicit operator Enum(ByteProperty prop) => throw new NotImplementedException();
	}

	public class ObjectProperty : Property
	{
		public int Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public static ObjectProperty Deserialize(BinaryReader br)
		{
			var prop = new ObjectProperty();
			prop.Value = br.ReadInt32();

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			bw.Write(Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator ObjectProperty(int value) => new ObjectProperty { Value = value };
		public static implicit operator int(ObjectProperty prop) => prop.Value;
	}

	public class StructProperty : Property
	{
		public string Type { get; set; }

		public byte[] Value { get; set; }

		public override object GetValue()
		{
			return ToString();
		}

		public static StructProperty Deserialize(BinaryReader br, long valueLength)
		{
			var prop = new StructProperty();
			prop.Type = "".Deserialize(br);
			prop.Value = br.ReadBytes((int)valueLength);

			return prop;
		}

		public override void Serialize(BinaryWriter bw)
		{
			Type.Serialize(bw);
			bw.Write(Value);
		}

		public override string ToString()
		{
			return $"{Type}:{Convert.ToHexString(Value)}";
		}

		public static implicit operator StructProperty((string, byte[]) value) => new StructProperty { Type = value.Item1, Value = value.Item2 };
		public static implicit operator (string, byte[])(StructProperty prop) => (prop.Type, prop.Value);
		public static implicit operator StructProperty(string value) => new StructProperty { Type = value.Split(":")[0], Value = Convert.FromHexString(value.Split(":")[1]) };
		public static implicit operator string(StructProperty prop) => prop.ToString();
	}
}
