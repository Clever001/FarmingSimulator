using FarmingClasses.Other;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FarmingClasses.Serialization;
public sealed class SavesController : IEnumerable<KeyValuePair<Player, GameSave>> {
    private SortedDictionary<Player, GameSave> _gameSaves = new();
    private readonly string _path;

    public SavesController(string path, bool loadSaves = false) {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

        _path = path;
        if (loadSaves) LoadSaves();
    }

    public int Count => _gameSaves.Count;

    public GameSave this[Player player] {
        get {
            if (ContainsThisPlayer(player)) return _gameSaves[player];
            else throw new ArgumentOutOfRangeException(nameof(player));
        }
    }

    public IEnumerable<Player> GetPlayers() => _gameSaves.Keys;

    public bool ContainsThisPlayer(Player player) => _gameSaves.ContainsKey(player);

    public bool LoadSaves(Player? player = null, GameSave? currentSave = null) {
        try {
            string json = File.ReadAllText(_path);
            var saves = JsonConvert.DeserializeObject<SortedDictionary<Player, GameSave>>(json) 
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

    public void SaveGame(Player? player = null, GameSave? save = null) {
        if (player is not null && save is not null) {
            if (ContainsThisPlayer(player)) {
                _gameSaves[player] = save;
            } else {
                _gameSaves.Add(player, save);
            }
        }

        JsonSerializerSettings settings = new JsonSerializerSettings {
            Converters = new List<JsonConverter> {
                new SortedDictionaryConverter<Player, GameSave>()
            },
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(_gameSaves, settings) 
            ?? throw new ArgumentException("Ошибка при сериализации объекта.");

        File.WriteAllText(_path, json);
    }

    public IEnumerator<KeyValuePair<Player, GameSave>> GetEnumerator() => _gameSaves.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
