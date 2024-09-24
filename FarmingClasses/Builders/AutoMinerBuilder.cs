using FarmingClasses.Other;

namespace FarmingClasses.Builders;
public class AutoMinerBuilder {
    public AutoMiner GetWaterer() => new("Поливальщик", 20, 2);

    public AutoMiner GetFertilizer() => new("Удобритель", 40, 4);

    public AutoMiner GetHarvester() => new("Сборщик урожая", 30, 3);

    public AutoMiner GetExpertGardener() => new("Садовод эксперт", 100, 15);
}
