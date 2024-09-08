using System;

namespace FarmingClasses.Other;

/// <summary>
/// Майнеры могут работать в саду.
/// </summary>
internal class AutoMiner : IBuyable {
    public string Name { get; }

    public int BaseCost { get; }

    /// <summary>
    /// Отображает сколько растений с огорода может собрать данный майнер.
    /// </summary>
    public int CanCollect { get; }

    public AutoMiner(string name, int baseCost, int canCollect) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(baseCost, nameof(baseCost));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(canCollect, nameof(canCollect));
        Name = name;
        BaseCost = baseCost;
        CanCollect = canCollect;
    }
}

