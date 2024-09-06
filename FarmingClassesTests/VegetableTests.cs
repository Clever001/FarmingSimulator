using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using FarmingClasses.Plants;

namespace FarmingClassesTests;

public class VegetableTests {
    [Fact]
    public void CreationTest() {
        Vegetable carrot = new("Carrot", new(2023, 2, 1), new(90), "Carrots are good for the eyes.", VegetableType.RootVegetables);
        Assert.Equal("Carrot", carrot.Name);
        Assert.Equal(new DateOnly(2023, 2, 1), carrot.PlantedTime);
        Assert.Equal(new Duration(90), carrot.MaturationTime);
        Assert.Equal("Carrots are good for the eyes.", carrot.Description);
        Assert.Equal(VegetableType.RootVegetables, carrot.VegetableType);

        Assert.Throws<ArgumentException>(() => { new Vegetable("", new(2023, 2, 1), new(90), "Carrots are good for the eyes.", VegetableType.RootVegetables); });
        Assert.Throws<ArgumentException>(() => { new Vegetable("  ", new(2023, 2, 1), new(90), "Carrots are good for the eyes.", VegetableType.RootVegetables); });
        Assert.Throws<ArgumentOutOfRangeException>(() => { new Vegetable("Carrot", default, new(90), "Carrots are good for the eyes.", VegetableType.RootVegetables); });
        Assert.Throws<DurationException>(() => { new Vegetable("Carrot", new(2023, 2, 1), default, "Carrots are good for the eyes.", VegetableType.RootVegetables); });
        Assert.Throws<ArgumentException>(() => { new Vegetable("Carrot", new(2023, 2, 1), new(90), "", VegetableType.RootVegetables); });
        Assert.Throws<ArgumentException>(() => { new Vegetable("Carrot", new(2023, 2, 1), new(90), " ", VegetableType.RootVegetables); });
    }

    [Fact]
    public void ValidRipeTest() {
        Vegetable carrot = new("Carrot", new(year:2023, month:2, day:1), new(30), "Carrots are good for the eyes.", VegetableType.RootVegetables);

        Assert.True(carrot.IsRipe(new(2023, 3, 3)));
        Assert.True(carrot.IsRipe(new(2023, 3, 4)));
        Assert.False(carrot.IsRipe(new(2023, 3, 2)));

        carrot = new("Carrot", new(year: 2023, month: 2, day: 1), new(60), "Carrots are good for the eyes.", VegetableType.RootVegetables);

        Assert.True(carrot.IsRipe(new(2023, 4, 2)));
        Assert.True(carrot.IsRipe(new(2023, 5, 2)));
        Assert.False(carrot.IsRipe(new(2023, 3, 2)));
    }
}
