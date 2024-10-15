using FarmingClasses.Other;
using System.Text.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FarmingClasses.Serialization;
public sealed class SavesController : IEnumerable<KeyValuePair<string, GameSave>> {
    private SortedDictionary<string, GameSave> _gameSaves = new();
    private readonly string _path;

    public SavesController(string path, bool loadSaves = false) {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        _path = path;
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
            string json = File.ReadAllText(_path);

            var saves = JsonSerializer.Deserialize<SortedDictionary<string, GameSave>>(json)
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

        string json = JsonSerializer.Serialize(_gameSaves)
            ?? throw new ArgumentException("Ошибка при сериализации объекта.");

        File.WriteAllText(_path, json);
    }

    public IEnumerator<KeyValuePair<string, GameSave>> GetEnumerator() => _gameSaves.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
