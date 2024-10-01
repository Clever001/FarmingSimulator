using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Builders;
public static class PlantBuilder {
    public static Plant GetPlant(string name) => name switch {
        "Яблоко" => (new FruitBuilder()).GetApple(),
        "Груша" => (new FruitBuilder()).GetPear(),
        "Черника" => (new FruitBuilder()).GetBlueberry(),
        "Картофель" => (new VegetableBuilder()).GetPotato(),
        "Морковь" => (new VegetableBuilder()).GetCarrot(),
        "Капуста" => (new VegetableBuilder()).GetCabbage(),
        _ => throw new ArgumentOutOfRangeException(nameof(name))
    };

    public static IEnumerable<T> GetRangeOfPlants<T>(T plant, int cnt, DateOnly date) where T : Plant {
        if (plant is Fruit) {
            FruitBuilder builder = new();
            List<Fruit> fruits = new();
            switch (plant.Name) {
                case "Яблоко":
                    for (int i = 0; i != cnt; i++) { fruits.Add(builder.GetApple(date)); }
                    break;
                case "Груша":
                    for (int i = 0; i != cnt; i++) { fruits.Add(builder.GetPear(date)); }
                    break;
                case "Черника":
                    for (int i = 0; i != cnt; i++) { fruits.Add(builder.GetBlueberry(date)); }
                    break;
                default:
                    throw new ArgumentException("Было неправильно интерпретировано название растения.");
            }
            return (IEnumerable<T>)fruits;
        } else if (plant is Vegetable) {
            VegetableBuilder builder = new();
            List<Vegetable> vegetables = new();
            switch (plant.Name) {
                case "Картофель":
                    for (int i = 0; i != cnt; i++) { vegetables.Add(builder.GetPotato(date)); }
                    break;
                case "Морковь":
                    for (int i = 0; i != cnt; i++) { vegetables.Add(builder.GetCarrot(date)); }
                    break;
                case "Капуста":
                    for (int i = 0; i != cnt; i++) { vegetables.Add(builder.GetCabbage(date)); }
                    break;
                default:
                    throw new ArgumentException("Было неправильно интерпретировано название растения.");
            }
            return (IEnumerable<T>)vegetables;
        }
        // Never executed;
        throw new Exception("Программе оказался неизвестный класс растений.");
    }
}
