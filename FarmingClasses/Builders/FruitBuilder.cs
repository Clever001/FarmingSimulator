using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;

namespace FarmingClasses.Builders;

public class FruitBuilder {
    private Random _random;

    public FruitBuilder(Random rnd) {
        _random = rnd;
    }

    public FruitBuilder() {
        _random = new();
    }

    public Plant GetApple(DateOnly plantedDate) =>
        new Fruit("Яблоко", plantedDate, new Duration(days: 45 + _random.Next(15)), "В реальности игры яблоки созревают за 45-60 дней.", TreeType.Deciduous);

    public Plant GetPear(DateOnly plantedDate) =>
        new Fruit("Груша", plantedDate, new Duration(days: 50 + _random.Next(20)), "В реальности игры груши вырастают за 50-70 дней.", TreeType.Deciduous));

    public Plant GetBlueberry(DateOnly plantedDate) =>
        new Fruit("Черника", plantedDate, new Duration(days: 50 + _random.Next(20)), "В реальности игры черника растет за 50-70 дней.", TreeType.Shrub);
}
