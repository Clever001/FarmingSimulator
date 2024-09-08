using System;

namespace FarmingClasses.Other;
public class Player {
    public string Name { get; }
    public int Capital { get; private set; }

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

    public bool IsBankrupt { get => Capital == 0; }

    public override string ToString() {
        return $"Игрок {Name} имеет при себе {Capital} денег.";
    }
}
