using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using FarmingClasses.Plants;

namespace FarmingClassesTests;

public class FruitTests {
    [Fact]
    public void CreationTest() {
        Fruit apple = new("Apple", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous);
        Assert.Equal("Apple", apple.Name);
        Assert.Equal(new DateOnly(2023, 2, 1), apple.PlantedTime);
        //Assert.Equal(new Duration(90), carrot.MaturationTime);
        Assert.Equal("Apple is the biggest software and hardware company. So... Apples are delicious.", apple.Description);
        Assert.Equal(TreeType.Deciduous, apple.TreeType);

        Assert.Throws<ArgumentException>(() => 
        { new Fruit("", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous); });
        Assert.Throws<ArgumentException>(() => 
        { new Fruit("  ", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous); });        
        Assert.Throws<ArgumentOutOfRangeException>(() => 
        { new Fruit("Apple", new(1840, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous); });
        Assert.Throws<DurationException>(() => 
        { new Fruit("Apple", new(2023, 2, 1), new(-5, 3), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous); });
        Assert.Throws<ArgumentException>(() => 
        { new Fruit("Apple", new(2023, 2, 1), new(90), "", TreeType.Deciduous); });
        Assert.Throws<ArgumentException>(() => 
        { new Fruit("Apple", new(2023, 2, 1), new(90), " ", TreeType.Deciduous); });
    }

    [Fact]
    public void ValidRipeTest() {
        Fruit apple = new("Apple", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous);

        Assert.True(apple.IsCollectable(new(2023, 5, 3)));
        Assert.True(apple.IsCollectable(new(2023, 5, 4)));
        Assert.False(apple.IsCollectable(new(2023, 3, 2)));

        apple = new("Apple", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous);

        Assert.True(apple.IsCollectable(new(2023, 5, 2)));
        Assert.True(apple.IsCollectable(new(2023, 5, 5)));
        Assert.False(apple.IsCollectable(new(2023, 3, 2)));
    }
}