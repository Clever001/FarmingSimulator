using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;

namespace FarmingClassesTests;

public class GardenTests {
    [Fact]
    public void AddFirst_AddsElementToBeginning() {
        // Arrange
        var garden = new Garden<Plant>();
        var plant = new Vegetable("Carrot", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Root vegetable", VegetableType.RootVegetables);

        // Act
        garden.AddFirst(plant);

        // Assert
        Assert.Single(garden);
        Assert.Equal(plant, garden.First());
    }

    [Fact]
    public void AddLast_AddsElementToEnd() {
        // Arrange
        var garden = new Garden<Plant>();
        var plant1 = new Vegetable("Tomato", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Fruit vegetable", VegetableType.FruitVegetables);
        var plant2 = new Vegetable("Potato", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 2), "Tuber crop", VegetableType.TuberCrop);

        // Act
        garden.AddLast(plant1);
        garden.AddLast(plant2);

        // Assert
        Assert.Equal(2, garden.Count);
        Assert.Equal(plant1, garden.First());
        Assert.Equal(plant2, garden.Last());
    }

    [Fact]
    public void AddRange_AddsMultipleElements() {
        // Arrange
        var garden = new Garden<Plant>();
        var plants = new List<Plant>
        {
            new Vegetable("Carrot", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Root vegetable", VegetableType.RootVegetables),
            new Vegetable("Lettuce", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Leafy vegetable", VegetableType.LeafyVegetables)
        };

        // Act
        garden.AddRange(plants);

        // Assert
        Assert.Equal(2, garden.Count);
        Assert.Contains(plants[0], garden);
        Assert.Contains(plants[1], garden);
    }

    [Fact]
    public void Remove_RemovesSpecifiedElement() {
        // Arrange
        var garden = new Garden<Plant>();
        var plant = new Vegetable("Cucumber", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Fruit vegetable", VegetableType.FruitVegetables);
        garden.Add(plant);

        // Act
        var result = garden.Remove(plant);

        // Assert
        Assert.True(result);
        Assert.Empty(garden);
    }

    [Fact]
    public void Contains_ReturnsTrueIfElementExists() {
        // Arrange
        var garden = new Garden<Plant>();
        var plant = new Fruit("Apple", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Deciduous tree", TreeType.Deciduous);
        garden.Add(plant);

        // Act
        var result = garden.Contains(plant);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Clear_RemovesAllElements() {
        // Arrange
        var garden = new Garden<Plant>();
        garden.Add(new Vegetable("Carrot", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Root vegetable", VegetableType.RootVegetables));

        // Act
        garden.Clear();

        // Assert
        Assert.Empty(garden);
    }

    [Fact]
    public void Sort_SortsElementsByName() {
        // Arrange
        var garden = new Garden<Plant>(new List<Plant>
        {
            new Vegetable("Zucchini", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Fruit vegetable", VegetableType.FruitVegetables),
            new Vegetable("Asparagus", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Flower vegetable", VegetableType.FlowerVegetables)
        });

        // Act
        garden.Sort();

        // Assert
        Assert.Equal("Asparagus", garden.First().Name);
        Assert.Equal("Zucchini", garden.Last().Name);
    }

    [Fact]
    public async Task SortAsync_SortsElementsByNameAsynchronously() {
        // Arrange
        var garden = new Garden<Plant>(new List<Plant>
        {
            new Fruit("Peach", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Deciduous tree", TreeType.Deciduous),
            new Fruit("Apple", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Deciduous tree", TreeType.Deciduous)
        });

        // Act
        await garden.SortAsync();

        // Assert
        Assert.Equal("Apple", garden.First().Name);
        Assert.Equal("Peach", garden.Last().Name);
    }

    [Fact]
    public void CopyTo_CopiesElementsToArray() {
        // Arrange
        var garden = new Garden<Plant>();
        var plants = new List<Plant>
        {
            new Vegetable("Carrot", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Root vegetable", VegetableType.RootVegetables),
            new Fruit("Apple", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Deciduous tree", TreeType.Deciduous)
        };
        garden.AddRange(plants);
        var array = new Plant[2];

        // Act
        garden.CopyTo(array, 0);

        // Assert
        Assert.Equal(plants[0].Name, array[0].Name);
        Assert.Equal(plants[1].Name, array[1].Name);
    }

    [Fact]
    public void RemoveIf_RemovesElementsSatisfyingCondition() {
        // Arrange
        var garden = new Garden<Plant>();
        garden.Add(new Vegetable("Carrot", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Root vegetable", VegetableType.RootVegetables));
        garden.Add(new Fruit("Apple", DateOnly.FromDateTime(DateTime.Now), new Duration(0, 1), "Deciduous tree", TreeType.Deciduous));

        // Act
        garden.RemoveIf(plant => plant.Name.StartsWith('A'));

        // Assert
        Assert.Single(garden);
        Assert.DoesNotContain(garden, p => p.Name.StartsWith('A'));
    }
}
