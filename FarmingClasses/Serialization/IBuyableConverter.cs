using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FarmingClasses.Serialization;
public class IBuyableConverter : JsonConverter<IBuyable> {
    public override IBuyable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string type = root.GetProperty("Key").GetString() ?? throw new ArgumentNullException();
        return type switch {
            "AutoMiner" => JsonSerializer.Deserialize<AutoMiner>(root.GetProperty("Value").GetRawText(), options) ?? throw new ArgumentNullException(),
            "Plant" => JsonSerializer.Deserialize<Plant>(root.GetProperty("Value").GetRawText(), options) ?? throw new ArgumentNullException(),
            _ => throw new JsonException($"Unknown IBuyable type: {type}"),
        };
    }

    public override void Write(Utf8JsonWriter writer, IBuyable value, JsonSerializerOptions options) {
        if (value is AutoMiner autoMiner) {
            JsonSerializer.Serialize(writer, new KeyValuePair<string, AutoMiner>("AutoMiner", autoMiner));
        }
        else if (value is Plant plant) {
            JsonSerializer.Serialize(writer, new KeyValuePair<string, Plant>("Plant", plant), new JsonSerializerOptions { Converters = { new PlantConverter() } });
        }
        else {
            throw new JsonException($"Unknown plant type: {value.GetType().Name}");
        }
    }
}
