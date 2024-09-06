using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;

public class VegetableBuilder {
    public Vegetable GetCarrot(DateOnly plantedTime, Duration maturationTime) {
        return new Vegetable("Carrot", plantedTime, maturationTime, "", VegetableType.RootVegetables);
    }

    public Vegetable GetLettuce(DateOnly plantedTime, Duration maturationTime) {
        return new Vegetable("Lettuce", plantedTime, maturationTime, "", VegetableType.LeafyVegetables);
    }

    public Vegetable GetWatermelon(DateOnly plantedTime, Duration maturationTime) {
        return new Vegetable("Watermelon", plantedTime, maturationTime, "", VegetableType.FruitVegetables);
    }
}
