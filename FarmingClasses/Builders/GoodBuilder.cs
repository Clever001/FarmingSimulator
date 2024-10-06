using FarmingClasses.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;

public static class GoodBuilder {
    public static IBuyable GetGood(string name) => name switch {
        "Яблоко" => FruitBuilder.GetApple(),
        "Груша" => FruitBuilder.GetPear(),
        "Черника" => FruitBuilder.GetBlueberry(),
        "Картофель" => VegetableBuilder.GetPotato(),
        "Морковь" => VegetableBuilder.GetCarrot(),
        "Капуста" => VegetableBuilder.GetCabbage(),
        "Поливальщик" => AutoMinerBuilder.GetWaterer(),
        "Удобритель" => AutoMinerBuilder.GetFertilizer(),
        "Сборщик урожая" => AutoMinerBuilder.GetHarvester(),
        "Садовод эксперт" => AutoMinerBuilder.GetExpertGardener(),
        _ => throw new ArgumentOutOfRangeException(nameof(name))
    };

    public static IEnumerable<IBuyable> GetAll() => PlantBuilder.GetAll().Concat<IBuyable>(AutoMinerBuilder.GetAll());
}
