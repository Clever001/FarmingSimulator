using FarmingClasses.Other;
using System.Collections.Generic;

namespace FarmingClasses.Builders;
public static class AutoMinerBuilder {
    public static AutoMiner GetWaterer() => new("Поливальщик", "Поливальщики", 20, 2);

    public static AutoMiner GetFertilizer() => new("Удобритель", "Удобрители", 40, 4);

    public static AutoMiner GetHarvester() => new("Сборщик урожая", "Сборщики урожая", 30, 3);

    public static AutoMiner GetExpertGardener() => new("Садовод эксперт", "Садоводы эксперты", 100, 15);

    public static IEnumerable<AutoMiner> GetAll() {
        List<AutoMiner> miners = [GetWaterer(), GetFertilizer(), GetHarvester(), GetExpertGardener()];
        return miners;
    }
}
