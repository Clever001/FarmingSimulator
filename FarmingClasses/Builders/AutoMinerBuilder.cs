using FarmingClasses.Other;
using System.Collections.Generic;

namespace FarmingClasses.Builders;
public class AutoMinerBuilder {
    public AutoMiner GetWaterer() => new("Поливальщик", "Поливальщики", 20, 2);

    public AutoMiner GetFertilizer() => new("Удобритель", "Удобрители", 40, 4);

    public AutoMiner GetHarvester() => new("Сборщик урожая", "Сборщики урожая", 30, 3);

    public AutoMiner GetExpertGardener() => new("Садовод эксперт", "Садоводы эксперты", 100, 15);

    public IEnumerable<IBuyable> GetAll() {
        List<IBuyable> miners = [GetWaterer(), GetFertilizer(), GetHarvester(), GetExpertGardener()];
        return miners;
    }
}
