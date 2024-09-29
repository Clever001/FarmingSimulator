using FarmingClasses.Other;
using System.Collections;
using System.Collections.Generic;

namespace FarmingClasses.Builders;
public class AutoMinerBuilder {
    public AutoMiner GetWaterer() => new("Поливальщик", 20, 2);

    public AutoMiner GetFertilizer() => new("Удобритель", 40, 4);

    public AutoMiner GetHarvester() => new("Сборщик урожая", 30, 3);

    public AutoMiner GetExpertGardener() => new("Садовод эксперт", 100, 15);

    public IEnumerable<IBuyable> GetAll() {
        List<IBuyable> miners = [GetWaterer(), GetFertilizer(), GetHarvester(), GetExpertGardener()];
        return miners;
    }
}
