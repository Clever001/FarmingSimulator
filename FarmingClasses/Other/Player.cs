using Newtonsoft.Json;
using System;

namespace FarmingClasses.Other;

public class Player : IComparable<Player>, IEquatable<Player> {
    public string Name { get; }
    public int Capital { get; set; }

    public Player(string name) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Capital = 100; // Изначально у каждого игрока есть 100 денежных единиц.
    }

    public void AddMoney(int amount) => Capital += amount;

    /// <summary>
    /// Снимает деньги со счета, если их достаточно.
    /// </summary>
    /// <returns>True если у игрока достаточно денег. Иначе False.</returns>
    public bool PayMoney(int amount) {
        if (Capital >= amount) {
            Capital -= amount;
            return true;
        }
        return false;
    }

    [JsonIgnore]
    public bool IsBankrupt { get => Capital == 0; }

    public void MakeBunkrupt() => Capital = 0;

    public override string ToString() {
        return $"Игрок {Name} имеет при себе {Capital} денег.";
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name);
    }

    public override bool Equals(object? obj) {
        if (obj is Player pl) return Equals(pl);
        else return false;
    }

    public int CompareTo(Player? other) {
        if (other is null) return 1;
        return Name.CompareTo(other.Name);
    }

    public bool Equals(Player? other) {
        if (other is null) return false;
        return Name.Equals(other.Name);
    }
}
