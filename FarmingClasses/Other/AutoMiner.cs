using System;
using System.Text.Json.Serialization;

namespace FarmingClasses.Other;

/// <summary>
/// Майнеры могут работать в саду.
/// </summary>
public class AutoMiner : IBuyable {
    public string Name { get; }
    public string? PluralName { get; } = null;
    public int BaseCost { get; }

    /// <summary>
    /// Отображает сколько растений с огорода может собрать данный майнер.
    /// </summary>
    public int CanCollect { get; }

    [JsonIgnore]
    public string Description => $"Всего может собрать: {CanCollect} урожая.";

    public AutoMiner(string name, int baseCost, int canCollect) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(baseCost, nameof(baseCost));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(canCollect, nameof(canCollect));
        Name = name;
        BaseCost = baseCost;
        CanCollect = canCollect;
    }

    [JsonConstructor]
    public AutoMiner(string name, string? pluralName, int baseCost, int canCollect)
        : this(name, baseCost, canCollect) {
        PluralName = pluralName;
    }

    public override int GetHashCode() => HashCode.Combine(Name, BaseCost);

    public bool Equals(IBuyable? other) {
        if (other is AutoMiner am) {
            return Name.Equals(am.Name, StringComparison.OrdinalIgnoreCase)
                   && BaseCost == am.BaseCost;
        }
        return false;
    }
}

