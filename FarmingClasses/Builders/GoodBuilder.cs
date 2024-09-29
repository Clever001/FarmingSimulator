using FarmingClasses.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;

public static class GoodBuilder {
    public static IBuyable GetGood(string name) => name switch {
        "Яблоко" => (new FruitBuilder()).GetApple(),
        "Груша" => (new FruitBuilder()).GetPear(),
        "Черника" => (new FruitBuilder()).GetBlueberry(),
        "Картофель" => (new VegetableBuilder()).GetPotato(),
        "Морковь" => (new VegetableBuilder()).GetCarrot(),
        "Капуста" => (new VegetableBuilder()).GetCabbage(),
        "Поливальщик" => (new AutoMinerBuilder()).GetWaterer(),
        "Удобритель" => (new AutoMinerBuilder()).GetFertilizer(),
        "Сборщик урожая" => (new AutoMinerBuilder()).GetHarvester(),
        "Садовод эксперт" => (new AutoMinerBuilder()).GetExpertGardener(),
        _ => throw new ArgumentOutOfRangeException(nameof(name))
    };
}
