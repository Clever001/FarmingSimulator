using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FarmingClasses.Serialization;
public class XmlSaver : ISavesController {
    private SortedDictionary<string, GameSave> _gameSaves = new();
    private readonly string _fullName;
    private readonly XmlSerializer _serializer;

    public XmlSaver(string fileName, bool loadSaves = false) {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName, nameof(fileName));

        _fullName = fileName + ".xml";
        _serializer = new XmlSerializer(typeof(List<KeyValuePair<string, GameSave>>));
        if (loadSaves) { LoadSaves(); }
    }

    public GameSave this[string name] {
        get {
            if (ContainsThisPlayer(name)) return _gameSaves[name];
            else throw new ArgumentOutOfRangeException(nameof(name));
        }
    }

    public int Count => _gameSaves.Count;

    public bool ContainsThisPlayer(string player) => _gameSaves.ContainsKey(player);

    public IEnumerator<KeyValuePair<string, GameSave>> GetEnumerator() => _gameSaves.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _gameSaves.GetEnumerator();

    public IEnumerable<string> GetPlayers() => _gameSaves.Keys;

    public bool LoadSaves(string? player = null, GameSave? currentSave = null) {
        try {
            using var fs = new FileStream(_fullName, FileMode.Open, FileAccess.Read);

            var preparedSaves = _serializer.Deserialize(fs) as List<KeyValuePair<string, GameSave>>;
            ArgumentNullException.ThrowIfNull(preparedSaves, nameof(preparedSaves));

            var saves = new SortedDictionary<string, GameSave>();
            foreach (var kvp in preparedSaves) { saves.Add(kvp.Key, kvp.Value); }

            if (player is not null && currentSave is not null) {
                if (saves.ContainsKey(player)) saves[player] = currentSave;
                else saves.Add(player, currentSave);
            }

            _gameSaves = saves;
            return true;
        }
        catch {
            return false;
        }
    }

    public void SaveGame(string? player = null, GameSave? save = null) {
        if (player is not null && save is not null) {
            if (ContainsThisPlayer(player)) {
                _gameSaves[player] = save;
            }
            else {
                _gameSaves.Add(player, save);
            }
        }

        using var fs = new FileStream(_fullName, FileMode.Create, FileAccess.Write);
        _serializer.Serialize(fs, _gameSaves.ToList());
    }
}
