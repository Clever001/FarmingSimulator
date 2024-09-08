using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;

public class VegetableBuilder {
    private Random _random;

    public VegetableBuilder(Random rnd) {
        _random = rnd;
    }

    public VegetableBuilder() {
        _random = new();
    }

    public Plant GetPotato(DateOnly plantedDate) => 
        new Vegetable("Картофель", plantedDate, new Duration(), "Картофель является первым овощем, который был выведен в космос! " +
            "Картофель стал символом надежды на то, что астронавты смогут иметь свежие продукты во время длительных космических путешествий!", VegetableType.TuberCrop);

    //public Plant GetCarrot(DateOnly plantedDate) =>
    //    new Vegetable("Морковь", plentedDate, maturationTime)
}
