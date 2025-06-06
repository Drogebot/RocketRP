using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class Actor : Core.Object
	{
		public virtual bool HasInitialPosition { get => true; }
		public virtual bool HasInitialRotation { get => false; }

		public Vector? InitialPosition { get; set; }
		public Rotator? InitialRotation { get; set; }

		public Rotator RelativeRotation { get; set; }
		public Vector RelativeLocation { get; set; }
		public Vector Velocity { get; set; }
		public ObjectTarget<Pawn> Instigator { get; set; }
		public bool bNetOwner { get; set; }
		public bool bBlockActors { get; set; }
		public bool bCollideWorld { get; set; }
		public bool bCollideActors { get; set; }
		public bool bHardAttach { get; set; }
		public bool bTearOff { get; set; }
		public bool bHidden { get; set; }
		public ObjectTarget<Actor> Base { get; set; }
		public ObjectTarget<Actor> Owner { get; set; }
		public ECollisionType ReplicatedCollisionType { get; set; }
		public ENetRole Role { get; set; }
		public ENetRole RemoteRole { get; set; }
		public EPhysics Physics { get; set; }
		public float DrawScale { get; set; }
		public Rotator Rotation { get; set; }
		public Vector Location { get; set; }

		public HashSet<int> SetPropertyObjectIndexes = new HashSet<int>();
		public HashSet<string> SetPropertyNames = new HashSet<string>();

		public void CalculatePropertyObjectIndexes(Replay replay)
		{
			foreach (var propName in SetPropertyNames)
			{
				var propType = GetType().GetProperty(propName);
				if (propType is null) throw new MissingMemberException($"Property {propName} not found in {GetType().Name}");

				var objectName = $"{propType.DeclaringType!.FullName!.Replace("RocketRP.Actors.", "")}:{propName}";
				var propObjectIndex = replay.Objects.IndexOf(objectName);
				if (propObjectIndex == -1) throw new KeyNotFoundException($"Object {objectName} not found in {nameof(replay.Objects)}");
				SetPropertyObjectIndexes.Add(propObjectIndex);
			}
		}

		public void Deserialize(BitReader br, Replay replay)
		{
			if (!HasInitialPosition) return;
			InitialPosition = Vector.Deserialize(br, replay);

			if (!HasInitialRotation) return;
			InitialRotation = Rotator.Deserialize(br);
		}

		public void DeserializeProperty(BitReader br, Replay replay, int propObjectIndex)
		{
			var propName = replay.Objects[propObjectIndex].Split(":").Last();
			SetPropertyObjectIndexes.Add(propObjectIndex);
			SetPropertyNames.Add(propName);

			var propertyInfo = GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo is null) throw new MissingMemberException($"Property {propName} not found in {GetType().Name}");

			var propType = propertyInfo.PropertyType;
			var valueType = propType;
			var valueIndex = 0;
			Array? arr = null;

			if (propType.IsEnum)
			{
				valueType = propType.GetEnumUnderlyingType();
			}

			else if (propType.IsArray)  //Fixed Size Array
			{
				var arrType = propType.GetElementType()!;
				valueType = Nullable.GetUnderlyingType(arrType)!;
				arr = (Array?)propertyInfo.GetValue(this);
				if (arr is null)
				{
					arr = Array.CreateInstance(arrType, propertyInfo.GetCustomAttribute<FixedArraySize>()!.Size);
					propertyInfo.SetValue(this, arr);
				}
				valueIndex = br.ReadInt32((uint)arr.Length);
			}

			object? value = null;

			if (valueType == typeof(bool)) value = br.ReadBit();
			else if (valueType == typeof(byte)) value = br.ReadByte();
			else if (valueType == typeof(int)) value = br.ReadInt32();
			else if (valueType == typeof(uint)) value = br.ReadUInt32();
			else if (valueType == typeof(long)) value = br.ReadInt64();
			else if (valueType == typeof(ulong)) value = br.ReadUInt64();
			else if (valueType == typeof(float)) value = br.ReadSingle();
			else if (valueType == typeof(string)) value = br.ReadString();
			else
			{
				var methodInfo = valueType.GetMethod("Deserialize", [typeof(BitReader)]) ?? valueType.GetMethod("Deserialize", [typeof(BitReader), typeof(Replay)]);
				if (methodInfo is null) throw new MissingMethodException($"Deserialize method in {valueType.Name} not found");
				else if (methodInfo.GetParameters().Length == 1) value = methodInfo.Invoke(null, [br]);
				else if (methodInfo.GetParameters().Length == 2) value = methodInfo.Invoke(null, [br, replay]);
			}

			if (propType.IsArray) arr!.SetValue(value, valueIndex);
			else propertyInfo.SetValue(this, value);
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			if (!InitialPosition.HasValue) return;
			InitialPosition.Value.Serialize(bw, replay);

			if (!InitialRotation.HasValue) return;
			InitialRotation.Value.Serialize(bw);
		}

		public void SerializeProperty(BitWriter bw, Replay replay, int propObjectIndex, int propId, uint maxPropId)
		{
			var propName = replay.Objects[propObjectIndex].Split(":").Last();

			var propertyInfo = GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo is null) throw new MissingMemberException($"Property {propName} not found in {GetType().Name}");

			var propType = propertyInfo.PropertyType;
			var valueType = propType;
			int maxValueIndex = 1;
			Array? arr = null;

			if (propType.IsEnum)
			{
				valueType = propType.GetEnumUnderlyingType();
			}

			else if (propType.IsArray)
			{
				var arrType = propType.GetElementType()!;
				valueType = Nullable.GetUnderlyingType(arrType)!;
				arr = (Array?)propertyInfo.GetValue(this);
				if (arr is null)
				{
					arr = Array.CreateInstance(arrType, propertyInfo.GetCustomAttribute<FixedArraySize>()!.Size);
					propertyInfo.SetValue(this, arr);
				}
				maxValueIndex = arr.Length;
			}

			MethodInfo? methodInfo = null;
			object? defaultValue = propType.IsArray && valueType.IsValueType ? Activator.CreateInstance(valueType) : null;
			var firstEntry = true;
			for (int valueIndex = 0; valueIndex < maxValueIndex; valueIndex++)
			{
				object? value;
				if (propType.IsArray)
				{
					value = arr!.GetValue(valueIndex);
					if (value is null) continue;
					if (!firstEntry)
					{
						bw.Write(true);
						bw.Write(propId, maxPropId);
					}
					firstEntry = false;
					bw.Write(valueIndex, (uint)maxValueIndex);
				}
				else value = propertyInfo.GetValue(this);

				if (valueType == typeof(bool)) bw.Write((bool)value!);
				else if (valueType == typeof(byte)) bw.Write((byte)value!);
				else if (valueType == typeof(int)) bw.Write((int)value!);
				else if (valueType == typeof(uint)) bw.Write((uint)value!);
				else if (valueType == typeof(long)) bw.Write((long)value!);
				else if (valueType == typeof(ulong)) bw.Write((ulong)value!);
				else if (valueType == typeof(float)) bw.Write((float)value!);
				else if (valueType == typeof(string)) bw.Write((string?)value);
				else
				{
					methodInfo = methodInfo ?? valueType.GetMethod("Serialize", [typeof(BitWriter)]) ?? valueType.GetMethod("Serialize", [typeof(BitWriter), typeof(Replay)]);
					if (methodInfo is null) throw new MissingMethodException($"Serialize method in {valueType.Name} not found");
					else if (methodInfo.GetParameters().Length == 1) methodInfo.Invoke(value, [bw]);
					else if (methodInfo.GetParameters().Length == 2) methodInfo.Invoke(value, [bw, replay]);
				}
			}
		}
	}
}
