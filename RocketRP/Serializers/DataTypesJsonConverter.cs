using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocketRP.DataTypes;
using RocketRP.DataTypes.TAGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RocketRP.Serializers
{
	public class DataTypesJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(GameServerID) || objectType == typeof(GameMode) || objectType == typeof(PartyLeader) || objectType == typeof(ReplicatedRBState) || objectType == typeof(ProductAttribute_TA) || objectType.IsSubclassOf(typeof(ProductAttribute_TA));
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (objectType == typeof(GameServerID))
			{
				var gameServerID = (GameServerID)(existingValue ?? new GameServerID());

				switch (reader.TokenType)
				{
					case JsonToken.String:
						gameServerID.Value = serializer.Deserialize<string>(reader);
						break;
					case JsonToken.Integer:
						gameServerID.Value = serializer.Deserialize<long>(reader).ToString();
						break;
					default:
						throw new JsonException($"Unexpected token type: {reader.TokenType}");
				}
				return gameServerID;
			}

			if (objectType == typeof(GameMode))
			{
				var gameMode = (GameMode)(existingValue ?? new GameMode());

				gameMode.Value = serializer.Deserialize<byte>(reader);
				return gameMode;
			}

			if (objectType == typeof(PartyLeader))
			{
				var partyLeader = (PartyLeader)(existingValue ?? new PartyLeader());

				partyLeader.UniqueId = serializer.Deserialize<UniqueNetId>(reader);
				return partyLeader;
			}

			if (objectType == typeof(ReplicatedRBState))
			{
				if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
				reader.Read();

				var rbState = (ReplicatedRBState)(existingValue ?? new ReplicatedRBState());
				while (reader.TokenType == JsonToken.PropertyName)
				{
					var propName = (string)reader.Value;
					reader.Read();
					switch (propName)
					{
						case "Sleeping":
							rbState.Sleeping = serializer.Deserialize<bool>(reader);
							break;
						case "Position":
							rbState.Position = serializer.Deserialize<Vector>(reader);
							break;
						case "Rotation":
							var jObject = serializer.Deserialize<JObject>(reader);
							if (jObject.ContainsKey("W")) rbState.Rotation = jObject.ToObject<Quat>();
							else rbState.Rotation = jObject.ToObject<Vector>();
							break;
						case "LinearVelocity":
							rbState.LinearVelocity = serializer.Deserialize<Vector>(reader);
							break;
						case "AngularVelocity":
							rbState.AngularVelocity = serializer.Deserialize<Vector>(reader);
							break;
						default:
							break;
					}
					reader.Read();
				}if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");
				return rbState;
			}

			if (objectType == typeof(ProductAttribute_TA))
			{
				if (reader.TokenType != JsonToken.StartObject) throw new JsonReaderException("Expected StartObject token!");
				reader.Read();

				ProductAttribute_TA attribute = default;
				ObjectTarget objectTarget = default;
				string className = string.Empty;

				bool keepReading = true;
				while (keepReading && reader.TokenType == JsonToken.PropertyName)
				{
					var propName = (string)reader.Value;
					reader.Read();
					switch (propName)
					{
						case "ObjectTarget":
							objectTarget = serializer.Deserialize<ObjectTarget>(reader);
							break;
						case "ClassName":
							className = serializer.Deserialize<string>(reader);
							break;
						default:
							keepReading = false;
							break;
					}
					if (keepReading) reader.Read();
				}

				switch (className)
				{
					case "TAGame.ProductAttribute_Painted_TA":
						var paintId = serializer.Deserialize<PaintColor>(reader);
						attribute = new ProductAttribute_Painted_TA(paintId);
						break;
					case "TAGame.ProductAttribute_SpecialEdition_TA":
						var editionId = serializer.Deserialize<SpecialEdition>(reader);
						attribute = new ProductAttribute_SpecialEdition_TA(editionId);
						break;
					case "TAGame.ProductAttribute_TeamEdition_TA":
						var teamEditionId = serializer.Deserialize<TeamEdition>(reader);
						attribute = new ProductAttribute_TeamEdition_TA(teamEditionId);
						break;
					case "TAGame.ProductAttribute_TitleID_TA":
						var title = serializer.Deserialize<string>(reader);
						attribute = new ProductAttribute_TitleID_TA(title);
						break;
					case "TAGame.ProductAttribute_UserColor_TA":
						var color = serializer.Deserialize<uint>(reader);
						attribute = new ProductAttribute_UserColor_TA(color);
						break;
					case "":
						throw new JsonException("Expected ClassName to be set!");
					default:
						throw new JsonException($"Unexpected class name: {className}");
				}
				reader.Read();

				attribute.ObjectTarget = objectTarget;
				attribute.ClassName = className;

				if (reader.TokenType != JsonToken.EndObject) throw new JsonReaderException("Expected EndObject token!");
				return attribute;
			}

			throw new JsonException($"Unexpected object type: {objectType}");
		}

		public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			if (value.GetType() == typeof(GameServerID))
			{
				var gameServerID = (GameServerID)value;

				serializer.Serialize(writer, gameServerID.Value);
				return;
			}

			if (value.GetType() == typeof(GameMode))
			{
				var gameMode = (GameMode)value;

				serializer.Serialize(writer, gameMode.Value);
				return;
			}

			if (value.GetType() == typeof(PartyLeader))
			{
				var partyLeader = (PartyLeader)value;

				serializer.Serialize(writer, partyLeader.UniqueId);
				return;
			}

			if (value.GetType() == typeof(ReplicatedRBState))
			{
				var rbState = (ReplicatedRBState)value;
				writer.WriteStartObject();

				writer.WriteKeyValue("Sleeping", rbState.Sleeping, serializer);
				writer.WriteKeyValue("Position", rbState.Position, serializer);
				writer.WriteKeyValue("Rotation", rbState.Rotation, serializer);
				writer.WriteKeyValue("LinearVelocity", rbState.LinearVelocity, serializer);
				writer.WriteKeyValue("AngularVelocity", rbState.AngularVelocity, serializer);

				writer.WriteEndObject();
				return;
			}

			if (value.GetType().IsSubclassOf(typeof(ProductAttribute_TA)))
			{
				var attribute = (ProductAttribute_TA)value;
				writer.WriteStartObject();

				writer.WriteKeyValue("ObjectTarget", attribute.ObjectTarget, serializer);
				writer.WriteKeyValue("ClassName", attribute.ClassName, serializer);
				switch (attribute.ClassName)
				{
					case "TAGame.ProductAttribute_Painted_TA":
						var painted = (ProductAttribute_Painted_TA)attribute;
						writer.WriteKeyValue("PaintId", painted.PaintId, serializer);
						break;
					case "TAGame.ProductAttribute_SpecialEdition_TA":
						var specialEdition = (ProductAttribute_SpecialEdition_TA)attribute;
						writer.WriteKeyValue("EditionId", specialEdition.EditionId, serializer);
						break;
					case "TAGame.ProductAttribute_TeamEdition_TA":
						var teamEdition = (ProductAttribute_TeamEdition_TA)attribute;
						writer.WriteKeyValue("EditionId", teamEdition.EditionId, serializer);
						break;
					case "TAGame.ProductAttribute_TitleID_TA":
						var titleID = (ProductAttribute_TitleID_TA)attribute;
						writer.WriteKeyValue("Title", titleID.Title, serializer);
						break;
					case "TAGame.ProductAttribute_UserColor_TA":
						var userColor = (ProductAttribute_UserColor_TA)attribute;
						writer.WriteKeyValue("Color", userColor.Color, serializer);
						break;
					default:
						throw new JsonException($"Unexpected class name: {attribute.ClassName}");
				}

				writer.WriteEndObject();
				return;
			}
		}
	}
}
