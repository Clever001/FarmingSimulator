using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FarmingClasses.Plants;

public class Vegetable : Plant
{
    public VegetableType VegetableType { get; }

    [JsonConstructor]
    public Vegetable(string name, DateOnly? plantedTime, Duration? maturationTime, string description, VegetableType vegetableType)
        : base(name, plantedTime, maturationTime, description)
    {
        VegetableType = vegetableType;
    }

    public override object Clone() => new Vegetable(Name, PlantedTime, MaturationTime, Description, VegetableType);
}

public enum VegetableType
{
    [Display(Name = "Корнеплодный")]
    RootVegetables,
    [Display(Name = "Плодовый")]
    FruitVegetables,
    [Display(Name = "Бобовый")]
    Legumes,
    [Display(Name = "Листовой")]
    LeafyVegetables,
    [Display(Name = "Цветочный")]
    FlowerVegetables,
    [Display(Name = "Клубнеплодный")]
    TuberCrop
}