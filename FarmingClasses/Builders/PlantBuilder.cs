using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;
public static class PlantBuilder {
    public static Plant GetPlant(string name) => name switch {
        "Яблоко" => (new FruitBuilder()).GetApple(),
        "Груша" => (new FruitBuilder()).GetPear(),
        "Черника" => (new FruitBuilder()).GetBlueberry(),
        "Картофель" => (new VegetableBuilder()).GetPotato(),
        "Морковь" => (new VegetableBuilder()).GetCarrot(),
        "Капуста" => (new VegetableBuilder()).GetCabbage(),
        _ => throw new ArgumentOutOfRangeException(nameof(name))
    };
}
