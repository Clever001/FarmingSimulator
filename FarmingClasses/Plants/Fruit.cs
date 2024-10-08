using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmingClasses.Plants;

public class Fruit : Plant
{

    public TreeType TreeType { get; }

    public Fruit(string name, DateOnly? plantedTime, Duration? maturationTime, string description, TreeType treeType)
        : base(name, plantedTime, maturationTime, description)
    {
        TreeType = treeType;
    }

    public override object Clone() => new Fruit(Name, PlantedTime, MaturationTime, Description, TreeType);
}

