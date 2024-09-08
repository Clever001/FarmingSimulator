using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;

namespace FarmingClasses.Plants;

public abstract class Plant : IBuyable, ICloneable, IEquatable<Plant> {
    public string Name { get; }

    public DateOnly PlantedTime { get; }

    protected Duration MaturationTime { get; }

    public string Description { get; }

    public int BaseCost => 10;

    public Plant(string name, DateOnly plantedTime, Duration maturationTime, string description) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        if (plantedTime.Year < 1900) throw new ArgumentOutOfRangeException(nameof(plantedTime), "The planting time is too early");
        if (maturationTime.Equals(default)) throw new DurationException(0, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        Name = name;
        PlantedTime = plantedTime;
        MaturationTime = maturationTime;
        Description = description;
    }

    public bool IsCollectable(DateOnly date) {
        DateOnly MatureDate = PlantedTime.AddDays(MaturationTime.Days).AddMonths(MaturationTime.Months);
        return MatureDate <= date;
    }

    public abstract object Clone();

    /// <summary>
    /// Проверяет равны ли два растения. 
    /// Проверяется только поле Name, так как другие поля не несут смысловой нагрузки.
    /// </summary>
    public bool Equals(Plant? other) {
        if (other is null) return false;

        return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
    }
}
