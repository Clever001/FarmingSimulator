using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;

namespace FarmingClasses.Plants;

public abstract class Plant : IBuyable, ICloneable, IEquatable<Plant> {
    public string Name { get; }

    public DateOnly? PlantedTime { get; private set; }

    protected Duration? MaturationTime { get; private set; }

    public string Description { get; }

    public int BaseCost => 10;

    public Plant(string name, DateOnly? plantedTime, Duration? maturationTime, string description) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        if (plantedTime?.Year < 1900) throw new ArgumentOutOfRangeException(nameof(plantedTime), "The planting time is too early");
        if (maturationTime.HasValue && maturationTime.Value.Days == 0 && maturationTime.Value.Months == 0) throw new DurationException(0, 0);
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        Name = name;
        PlantedTime = plantedTime;
        MaturationTime = maturationTime;
        Description = description;
    }

    public void ChangePlantAndMaturationTime(DateOnly planted, Duration maturation) {
        PlantedTime = planted;
        MaturationTime = maturation;
    }

    public bool IsCollectable(DateOnly date) {
        ArgumentNullException.ThrowIfNull(MaturationTime, nameof(MaturationTime));
        if (PlantedTime is null) return false;
        DateOnly MatureDate = PlantedTime.Value.AddDays(MaturationTime.Value.Days).AddMonths(MaturationTime.Value.Months);
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

    public bool Equals(IBuyable? other) {
        if (other is Plant pl) {
            return Name.Equals(pl.Name, StringComparison.OrdinalIgnoreCase)
                   && BaseCost == pl.BaseCost;
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name, BaseCost);
    }
}
