using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmingClasses.Plants;

public class Fruit : Plant {

    public TreeType TreeType { get; }

    public Fruit(string name, DateOnly? plantedTime, Duration maturationTime, string description, TreeType treeType)
        : base(name, plantedTime, maturationTime, description) {
        TreeType = treeType;
    }

    public override object Clone() => new Fruit(Name, PlantedTime, MaturationTime, Description, TreeType);
}

public enum TreeType {
    [Display(Name = "Лиственное дерево")]
    Deciduous,
    [Display(Name = "Хвойное дерево")]
    Coniferous,
    [Display(Name = "Кустарник")]
    Shrub,
    [Display(Name = "Лиана")]
    Vine,
    [Display(Name = "Пальма")]
    Palm,
    [Display(Name = "Цитрусовое дерево")]
    Citrus
}
