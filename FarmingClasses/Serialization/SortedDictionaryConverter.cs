using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class SortedDictionaryConverter<TKey, TValue> : JsonConverter where TKey : notnull {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(SortedDictionary<TKey, TValue>);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var dictionary = (SortedDictionary<TKey, TValue>)value;
        writer.WriteStartObject();

        foreach (var kvp in dictionary) {
            writer.WritePropertyName(JsonConvert.SerializeObject(kvp.Key, Formatting.None));
            serializer.Serialize(writer, kvp.Value);
        }

        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        var dictionary = new SortedDictionary<TKey, TValue>();

        // Ожидаем начало объекта
        if (reader.TokenType != JsonToken.StartObject) {
            throw new JsonSerializationException($"Expected StartObject, got {reader.TokenType}");
        }

        // Перемещаемся внутрь объекта
        reader.Read();

        while (reader.TokenType != JsonToken.EndObject) {
            // Считываем строковое представление ключа
            string keyString = reader.Value as string ?? throw new ArgumentNullException();

            // Десериализуем ключ из строкового представления
            TKey key = JsonConvert.DeserializeObject<TKey>(keyString) ?? throw new ArgumentNullException();

            // Читаем следующее значение
            reader.Read();

            // Десериализуем значение
            TValue value = serializer.Deserialize<TValue>(reader) ?? throw new ArgumentNullException();

            // Добавляем ключ и значение в словарь
            dictionary.Add(key, value);

            // Переходим к следующей паре ключ-значение
            reader.Read();
        }

        return dictionary;
    }
}

