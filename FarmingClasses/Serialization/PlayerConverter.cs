using Newtonsoft.Json;
using System;

namespace FarmingClasses.Other {
    public class PlayerConverter : JsonConverter<Player> {
        public override void WriteJson(JsonWriter writer, Player? value, JsonSerializer serializer) {
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteValue(value.Name);
            writer.WritePropertyName("Capital");
            writer.WriteValue(value.Capital);
            writer.WriteEndObject();
        }

        public override Player ReadJson(JsonReader reader, Type objectType, Player? existingValue, bool hasExistingValue, JsonSerializer serializer) {
            if (reader.TokenType != JsonToken.StartObject) {
                throw new JsonSerializationException("Expected StartObject token");
            }

            reader.Read(); // Move to the first property (Name)

            string? propertyName = reader.Value?.ToString();
            if (propertyName != "Name") {
                throw new JsonSerializationException("Expected property 'Name'");
            }

            reader.Read(); // Move to the value of 'Name'
            string? name = reader.Value?.ToString();

            reader.Read(); // Move to the next property (Capital)

            propertyName = reader.Value?.ToString();
            if (propertyName != "Capital") {
                throw new JsonSerializationException("Expected property 'Capital'");
            }

            reader.Read(); // Move to the value of 'Capital'
            int capital = Convert.ToInt32(reader.Value);

            reader.Read(); // Move to EndObject

            return new Player(name!) {
                Capital = capital
            };
        }
    }
}
