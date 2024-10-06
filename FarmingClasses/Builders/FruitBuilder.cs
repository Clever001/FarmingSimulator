using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FarmingClasses.Builders;

public class FruitBuilder {
    private Random _random;

    public FruitBuilder(Random rnd) {
        _random = rnd;
    }

    public FruitBuilder() {
        _random = new();
    }

    public Fruit GetApple(DateOnly? plantedDate) =>
        new Fruit("Яблоко", plantedDate, new Duration(days: 45 + _random.Next(15)), "В реальности игры яблоки созревают за 45-60 дней.", TreeType.Deciduous);
    public static Fruit GetApple() =>
        new Fruit("Яблоко", null, null, "В реальности игры яблоки созревают за 45-60 дней.", TreeType.Deciduous);

    public Fruit GetPear(DateOnly? plantedDate) =>
        new Fruit("Груша", plantedDate, new Duration(days: 50 + _random.Next(20)), "В реальности игры груши вырастают за 50-70 дней.", TreeType.Deciduous);
    public static Fruit GetPear() =>
        new Fruit("Груша", null, null, "В реальности игры груши вырастают за 50-70 дней.", TreeType.Deciduous);

    public Fruit GetBlueberry(DateOnly? plantedDate = null) =>
        new Fruit("Черника", plantedDate, new Duration(days: 50 + _random.Next(20)), "В реальности игры черника растет за 50-70 дней.", TreeType.Shrub);
    public static Fruit GetBlueberry() =>
        new Fruit("Черника", null, null, "В реальности игры черника растет за 50-70 дней.", TreeType.Shrub);

    public static IEnumerable<Fruit> GetAll() {
        List<Fruit> fruits = [GetApple(), GetPear(), GetBlueberry()];
        return fruits;
    }
}
