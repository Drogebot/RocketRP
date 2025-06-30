using RocketRP.DataTypes;
using RocketRP.DataTypes.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Actors.Engine
{
	public class Actor : Core.Object
	{
		public virtual bool HasInitialPosition => true;
		public virtual bool HasInitialRotation => false;
		public HashSet<(ClassNetCacheProperty Property, int? Index)> SetProperties = [];

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

		public static Dictionary<Type, Func<Actor>>? InstanceCreatorCache;
		[MethodImpl(MethodImplOptions.Synchronized), MemberNotNull(nameof(InstanceCreatorCache))]
		public static void GenerateActorInstanceCreatorCache()
		{
			if (InstanceCreatorCache is not null) return;
			InstanceCreatorCache = Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Actor))).ToDictionary(t => t, t => Expression.Lambda<Func<Actor>>(Expression.New(t)).Compile());
		}

		public static Actor CreateInstance(Type actorType)
		{
			if (InstanceCreatorCache is null) GenerateActorInstanceCreatorCache();
			return InstanceCreatorCache[actorType]();
		}

		public Dictionary<string, object> ToDictionary(IEnumerable<(ClassNetCacheProperty Property, int Index)> properties)
		{
			var props = new Dictionary<string, object>();
			foreach (var propInfo in properties)
			{
				if (!props.ContainsKey(propInfo.Property.PropertyInfo.Name))
				{
					props.Add(propInfo.Property.PropertyInfo.Name, propInfo.Property.PropertyInfo.GetValue(this)!);
				}
			}
			return props;
		}
	}
}
