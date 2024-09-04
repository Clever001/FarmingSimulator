using FarmingClasses.Other;
using FarmingClasses.Exceptions;

namespace FarmingClassesTests;

public class DurationTests {
    [Fact]
    public void CreationTest() {
        Duration d = new(4, 0);
        Assert.Equal(4, d.Days);
        Assert.Equal(0, d.Months);
        Assert.Throws<DurationException>(() => { new Duration(-1, 4); });
        Assert.Throws<DurationException>(() => { new Duration(2, -34); });
        Assert.Throws<DurationException>(() => { new Duration(-4, -5); });
        Assert.Throws<DurationException>(() => { new Duration(0, 0); });
    }

    [Fact]
    public void EqualityTest() {
        Duration d1 = new(4, 0);
        Duration d2 = new(4, 0);
        Assert.Equal(d1, d2);
        d2 = new(3, 2);
        Assert.NotEqual(d1, d2);
    }

    [Fact]
    public void ComparisonTest() {
        Duration d1 = new(days: 0, months: 4);
        Duration d2 = new(0, 4);
        Assert.Equal(0, d1.CompareTo(d2));

        d1 = new(2, 3);
        Assert.True(d1.CompareTo(d2) < 0);
        d2 = new(0, 3);
        Assert.True(d1.CompareTo(d2) > 0);
    }
    
}

