using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FarmingClasses.Serialization;
public sealed class JsonSaver : ISavesController {
    private SortedDictionary<string, GameSave> _gameSaves = new();
    private readonly string _fullName;

    public JsonSaver(string fileName, bool loadSaves = false) {
        ArgumentException.ThrowIfNullOrEmpty(fileName, nameof(fileName));

        _fullName = fileName + ".json";
        if (loadSaves) LoadSaves();
    }

    public int Count => _gameSaves.Count;

    public GameSave this[string name] {
        get {
            if (ContainsThisPlayer(name)) return _gameSaves[name];
            else throw new ArgumentOutOfRangeException(nameof(name));
        }
    }

    public IEnumerable<string> GetPlayers() => _gameSaves.Keys;

    public bool ContainsThisPlayer(string player) => _gameSaves.ContainsKey(player);

    public bool LoadSaves(string? player = null, GameSave? currentSave = null) {
        try {
            string json = File.ReadAllText(_fullName);

            var saves = JsonSerializer.Deserialize<SortedDictionary<string, GameSave>>(json, new JsonSerializerOptions { Converters = { new ConverterOfPlants(), new ConverterOfIBuyable() } })
                ?? throw new ArgumentNullException("Ошибка в чтении json файла.");

            if (player is not null && currentSave is not null) {
                if (saves.ContainsKey(player)) saves[player] = currentSave;
                else saves.Add(player, currentSave);
            }

            _gameSaves = saves;
            return true;
        } catch {
            return false;
        }
    }

    public void SaveGame(string? player = null, GameSave? save = null) {
        if (player is not null && save is not null) {
            if (ContainsThisPlayer(player)) {
                _gameSaves[player] = save;
            } else {
                _gameSaves.Add(player, save);
            }
        }

        string json = JsonSerializer.Serialize(_gameSaves, new JsonSerializerOptions { Converters = { new ConverterOfPlants(), new ConverterOfIBuyable() } })
            ?? throw new ArgumentException("Ошибка при сериализации объекта.");

        File.WriteAllText(_fullName, json);
    }

    public IEnumerator<KeyValuePair<string, GameSave>> GetEnumerator() => _gameSaves.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
