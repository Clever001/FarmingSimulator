using FarmingClasses.Exceptions;
using FarmingClasses.Other;
using FarmingClasses.Plants;

namespace FarmingClassesTests;

public class FruitTests {
    [Fact]
    public void CreationTest() {
        Fruit carrot = new("Apple", new(2023, 2, 1), new(90), "Apple is the biggest software and hardware company. So... Apples are delicious.", TreeType.Deciduous);
        Assert.Equal("Apple", carrot.Name);
        Assert.Equal(new DateOnly(2023, 2, 1), carrot.PlantedTime);
        //Assert.Equal(new Duration(90), carrot.MaturationTime);
        Assert.Equal("Apple is the biggest software and hardware company. So... Apples are delicious.", carrot.Description);
        Assert.Equal(TreeType.Deciduous, carrot.TreeType);

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
}