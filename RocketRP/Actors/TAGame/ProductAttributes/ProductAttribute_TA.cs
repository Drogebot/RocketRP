using RocketRP.Actors.Core;
using System;
using System.Text.Json.Serialization;

namespace RocketRP.Actors.TAGame
{
	[JsonPolymorphic(TypeDiscriminatorPropertyName = "ClassName")]
	[JsonDerivedType(typeof(ProductAttribute_Blueprint_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_Blueprint_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_BlueprintCost_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_BlueprintCost_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_Certified_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_Certified_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_NoNotify_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_NoNotify_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_Painted_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_Painted_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_Quality_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_Quality_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_SpecialEdition_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_SpecialEdition_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_TeamEdition_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_TeamEdition_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_TitleID_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_TitleID_TA)}")]
	[JsonDerivedType(typeof(ProductAttribute_UserColor_TA), $"{nameof(TAGame)}.{nameof(ProductAttribute_UserColor_TA)}")]
	public abstract class ProductAttribute_TA : Core.Object
	{
		[JsonPropertyOrder(-2)]
		public ObjectTarget<ClassObject> ObjectTarget { get; set; }
		[JsonIgnore]
		public string? ClassName { get; set; }

		public static ProductAttribute_TA Deserialize(BitReader br, Replay replay)
		{
			var objectTarget = ObjectTarget<ClassObject>.Deserialize(br, replay);
			var className = replay.Objects[objectTarget.TargetIndex];

			var type = Type.GetType($"RocketRP.Actors.{className}");
			var attribute = (ProductAttribute_TA?)type?.GetMethod("DeserializeType")?.Invoke(null, [br, replay]) ?? throw new NullReferenceException();

			attribute.ObjectTarget = objectTarget;
			attribute.ClassName = className;

			return attribute;
		}

		public virtual void Serialize(BitWriter bw, Replay replay)
		{
			ObjectTarget.Serialize(bw, replay);
		}
	}
}
