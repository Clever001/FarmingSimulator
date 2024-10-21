using FarmingClasses.Plants;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace FarmingClasses.Serialization;
public class ConverterOfPlants : JsonConverter<Plant> {
    public override Plant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string type = root.GetProperty("Key").GetString() ?? throw new ArgumentNullException();
        return type switch {
            "Vegetable" => JsonSerializer.Deserialize<Vegetable>(root.GetProperty("Value").GetRawText(), options) ?? throw new ArgumentNullException(),
            "Fruit" => JsonSerializer.Deserialize<Fruit>(root.GetProperty("Value").GetRawText(), options) ?? throw new ArgumentNullException(),
            _ => throw new JsonException($"Unknown plant type: {type}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, Plant value, JsonSerializerOptions options) {
        if (value is Vegetable vegetable) {
            JsonSerializer.Serialize(writer, new KeyValuePair<string, Vegetable>("Vegetable", vegetable));
        }
        else if (value is Fruit fruit) {
            JsonSerializer.Serialize(writer, new KeyValuePair<string, Fruit>("Fruit", fruit));
        }
        else {
            throw new JsonException($"Unknown plant type: {value.GetType().Name}");
        }
    }
}
