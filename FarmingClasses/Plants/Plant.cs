using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;

namespace FarmingClasses.Plants;

public abstract class Plant {
    public string Name { get; }

    public DateOnly PlantedTime { get; }

    private Duration MaturationTime { get; }

    public string Description { get; }

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

}