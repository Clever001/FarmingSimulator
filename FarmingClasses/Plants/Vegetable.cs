using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmingClasses.Plants;
public class Vegetable : Plant {
    public VegetableType Type { get; }

    public Vegetable(string name, DateOnly plantedTime, Duration maturationTime, string description, VegetableType type) 
        : base(name, plantedTime, maturationTime, description) {
        Type = type;
    }
}

public enum VegetableType {
    [Display(Name = "Корнеплодный")]
    RootVegetables,
    [Display(Name = "Плодовый")]
    FruitVegetables,
    [Display(Name = "Бобовый")]
    Legumes,
    [Display(Name = "Листовой")]
    LeafyVegetables,
    [Display(Name = "Цветочный")]
    FlowerVegetables
}