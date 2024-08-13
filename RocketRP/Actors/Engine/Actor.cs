using RocketRP.Actors.Core;
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
		public ObjectTarget Instigator { get; set; }
		public bool bNetOwner { get; set; }
		public bool bBlockActors { get; set; }
		public bool bCollideWorld { get; set; }
		public bool bCollideActors { get; set; }
		public bool bHardAttach { get; set; }
		public bool bTearOff { get; set; }
		public bool bHidden { get; set; }
		public ObjectTarget Base { get; set; }
		public ObjectTarget Owner { get; set; }
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
				var test = GetType().GetProperty(propName);
				var propObjectIndex = replay.Objects.IndexOf($"{GetType().GetProperty(propName).DeclaringType.FullName.Replace("RocketRP.Actors.", "")}:{propName}");
				if (propObjectIndex == -1) throw new Exception($"Property {propName} not found in {GetType().Name}");
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
			SetPropertyObjectIndexes.Add(propObjectIndex);

			var propName = replay.Objects[propObjectIndex].Split(":").Last();
			SetPropertyNames.Add(propName);

			var propertyInfo = GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Field {propName} not found in {GetType().Name}");

			if (propertyInfo.PropertyType == typeof(bool)) propertyInfo.SetValue(this, br.ReadBit());
			else if (propertyInfo.PropertyType == typeof(byte)) propertyInfo.SetValue(this, br.ReadByte());
			else if (propertyInfo.PropertyType == typeof(int)) propertyInfo.SetValue(this, br.ReadInt32());
			else if (propertyInfo.PropertyType == typeof(uint)) propertyInfo.SetValue(this, br.ReadUInt32());
			else if (propertyInfo.PropertyType == typeof(long)) propertyInfo.SetValue(this, br.ReadInt64());
			else if (propertyInfo.PropertyType == typeof(ulong)) propertyInfo.SetValue(this, br.ReadUInt64());
			else if (propertyInfo.PropertyType == typeof(float)) propertyInfo.SetValue(this, br.ReadSingle());
			else if (propertyInfo.PropertyType == typeof(string)) propertyInfo.SetValue(this, br.ReadString());

			else if (propertyInfo.PropertyType.IsEnum)
			{   // I've never tested this, but it should work
				var enumType = propertyInfo.PropertyType.GetEnumUnderlyingType();

				if (enumType == typeof(byte)) propertyInfo.SetValue(this, Convert.ChangeType(br.ReadByte(), enumType));
				else if (enumType == typeof(int)) propertyInfo.SetValue(this, Convert.ChangeType(br.ReadInt32(), enumType));
				else if (enumType == typeof(uint)) propertyInfo.SetValue(this, Convert.ChangeType(br.ReadUInt32(), enumType));
				else if (enumType == typeof(long)) propertyInfo.SetValue(this, Convert.ChangeType(br.ReadInt64(), enumType));
				else if (enumType == typeof(ulong)) propertyInfo.SetValue(this, Convert.ChangeType(br.ReadUInt64(), enumType));
			}

			else if (propertyInfo.PropertyType.GetInterface("IArrayProperty") == typeof(IArrayProperty))
			{
				((IArrayProperty)propertyInfo.GetValue(this)).Deserialize(br, replay);
			}

			else
			{
				var methodInfo = propertyInfo.PropertyType.GetMethod("Deserialize", new Type[] { typeof(BitReader) }) ?? propertyInfo.PropertyType.GetMethod("Deserialize", new Type[] { typeof(BitReader), typeof(Replay) });
				if (methodInfo.GetParameters().Length == 1) propertyInfo.SetValue(this, methodInfo.Invoke(null, new object[] { br }));
				else if (methodInfo.GetParameters().Length == 2) propertyInfo.SetValue(this, methodInfo.Invoke(null, new object[] { br, replay }));
				else throw new MethodAccessException($"Deserialize method in {propertyInfo.PropertyType.Name} must have 1 or 2 parameters");
			}
		}

		public void Serialize(BitWriter bw, Replay replay)
		{
			if (!InitialPosition.HasValue) return;
			InitialPosition.Value.Serialize(bw, replay);

			if (!InitialRotation.HasValue) return;
			InitialRotation.Value.Serialize(bw);
		}

		public void SerializeProperty(BitWriter bw, Replay replay, int propId, int propObjectIndex)
		{
			var propName = replay.Objects[propObjectIndex].Split(":").Last();

			var propertyInfo = GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo == null) throw new Exception($"Field {propName} not found in {GetType().Name}");

			if (propertyInfo.PropertyType == typeof(bool)) bw.Write((bool)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(byte)) bw.Write((byte)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(int)) bw.Write((int)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(uint)) bw.Write((uint)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(long)) bw.Write((long)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(ulong)) bw.Write((ulong)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(float)) bw.Write((float)propertyInfo.GetValue(this));
			else if (propertyInfo.PropertyType == typeof(string)) bw.Write((string)propertyInfo.GetValue(this));

			else if (propertyInfo.PropertyType.IsEnum)
			{   // I've never tested this, but it should work
				var enumType = propertyInfo.PropertyType.GetEnumUnderlyingType();

				if (enumType == typeof(byte)) bw.Write((byte)propertyInfo.GetValue(this));
				else if (enumType == typeof(int)) bw.Write((int)propertyInfo.GetValue(this));
				else if (enumType == typeof(uint)) bw.Write((uint)propertyInfo.GetValue(this));
				else if (enumType == typeof(long)) bw.Write((long)propertyInfo.GetValue(this));
				else if (enumType == typeof(ulong)) bw.Write((ulong)propertyInfo.GetValue(this));
			}

			else if (propertyInfo.PropertyType.GetInterface("IArrayProperty") == typeof(IArrayProperty))
			{
				var classNetCache = replay.ClassNetCacheByName[GetType().FullName.Replace("RocketRP.Actors.", "")];
				((IArrayProperty)propertyInfo.GetValue(this)).Serialize(bw, replay, propId, classNetCache.NumProperties);
			}

			else
			{
				var methodInfo = propertyInfo.PropertyType.GetMethod("Serialize", new Type[] { typeof(BitWriter) }) ?? propertyInfo.PropertyType.GetMethod("Serialize", new Type[] { typeof(BitWriter), typeof(Replay) });
				if (methodInfo.GetParameters().Length == 1) methodInfo.Invoke(propertyInfo.GetValue(this), new object[] { bw });
				if (methodInfo.GetParameters().Length == 2) methodInfo.Invoke(propertyInfo.GetValue(this), new object[] { bw, replay });
			}
		}
	}
}
