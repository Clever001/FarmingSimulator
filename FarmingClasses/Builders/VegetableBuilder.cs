using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;

namespace FarmingClasses.Builders;

public class VegetableBuilder {
    private Random _random;

    public VegetableBuilder(Random rnd) {
        _random = rnd;
    }

    public VegetableBuilder() {
        _random = new();
    }

    public Vegetable GetPotato(DateOnly? plantedDate = null) => 
        new Vegetable("Картофель", plantedDate, new Duration(days: 90/* + _random.Next(31)*/), "Картофель обычно растет от 90 до 120 дней.", VegetableType.TuberCrop);

    public Vegetable GetCarrot(DateOnly? plantedDate = null) =>
        new Vegetable("Морковь", plantedDate, new Duration(days: _random.Next(5), months: 2 + _random.Next(1)), "Морковь растет от 2 до 3 месяцев.", VegetableType.RootVegetables);

    public Vegetable GetCabbage(DateOnly? plantedDate = null) =>
        new Vegetable("Капуста", plantedDate, new Duration(days: 45 + _random.Next(15)), "Капуста растет от 45 до 60 дней.", VegetableType.LeafyVegetables);

    public IEnumerable<IBuyable> GetAll() {
        List<IBuyable> vegetables = [GetPotato(), GetCarrot(), GetCabbage()];
        return vegetables;
    }
}
