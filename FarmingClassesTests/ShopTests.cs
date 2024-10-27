using System;
using System.Collections.Generic;
using Xunit;
using FarmingClasses.Collections;
using FarmingClasses.Other;

namespace FarmingClassesTests;
public class ShopTests {
    private class MockBuyable : IBuyable {
        public string Name { get; }
        public int BaseCost { get; }
        public string Description { get; }

        public MockBuyable(string name, int baseCost, string description = "") {
            Name = name;
            BaseCost = baseCost;
            Description = description;
        }

        public override int GetHashCode() => HashCode.Combine(Name);

        public override bool Equals(object? obj) {
            if (obj is IBuyable buyable) return Equals(buyable);
            return false;
        }

        public bool Equals(IBuyable? other) {
            return other is not null && Name.Equals(other.Name);
        }
    }

    private class MockPlayer : Player {
        public MockPlayer(string name, int capital) : base(name) {
            Capital = capital;
        }
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenGoodsIsNull() {
        Assert.Throws<ArgumentNullException>(() => new Shop((IEnumerable<IBuyable>)null!));
    }

    [Fact]
    public void Constructor_ShouldAddItems_WithInitialCost() {
        var goods = new List<IBuyable> {
            new MockBuyable("Apple", 10),
            new MockBuyable("Banana", 15)
        };

        var shop = new Shop(goods);

        Assert.Equal(10, shop[goods[0]]);
        Assert.Equal(15, shop[goods[1]]);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenItemsIsNull() {
        Assert.Throws<ArgumentNullException>(() => new Shop((IEnumerable<KeyValuePair<IBuyable, int>>)null!));
    }

    [Fact]
    public void Indexer_ShouldReturnCorrectCost() {
        var apple = new MockBuyable("Apple", 10);
        var shop = new Shop(new List<IBuyable> { apple });

        Assert.Equal(10, shop[apple]);
    }

    [Fact]
    public void Buy_ShouldReturnTrue_AndIncreaseCost_WhenPlayerCanAfford() {
        var apple = new MockBuyable("Apple", 10);
        var player = new MockPlayer("John", 20);
        var shop = new Shop(new List<IBuyable> { apple });

        var result = shop.Buy(apple, player);

        Assert.True(result);
        Assert.Equal(10, player.Capital); // 20 - 10 = 10
        Assert.Equal(11, shop[apple]); // 10 * 1.05 = 10.5 -> округляется до 11
    }

    [Fact]
    public void Buy_ShouldReturnFalse_WhenPlayerCannotAfford() {
        var apple = new MockBuyable("Apple", 10);
        var player = new MockPlayer("John", 5); // Недостаточно денег
        var shop = new Shop(new List<IBuyable> { apple });

        var result = shop.Buy(apple, player);

        Assert.False(result);
        Assert.Equal(5, player.Capital); // Деньги не изменились
        Assert.Equal(10, shop[apple]); // Стоимость не изменилась
    }

    [Fact]
    public void Buy_ShouldReturnFalse_WhenGoodIsNotInShop() {
        var apple = new MockBuyable("Apple", 10);
        var player = new MockPlayer("John", 20);
        var shop = new Shop(new List<IBuyable> { });

        var result = shop.Buy(apple, player);

        Assert.False(result);
        Assert.Equal(20, player.Capital); // Деньги не изменились
    }

    [Fact]
    public void GetGoods_ShouldReturnAllGoods() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var shop = new Shop(new List<IBuyable> { apple, banana });

        var goods = shop.GetGoods();

        Assert.Contains(apple, goods);
        Assert.Contains(banana, goods);
    }

    [Fact]
    public void Shop_ShouldBeSorted_ByComparerLogic() {
        var apple = new MockBuyable("Apple", 10);
        var autoMiner = new MockBuyable("AutoMiner", 20);
        var shop = new Shop(new List<IBuyable> { autoMiner, apple });

        var goods = shop.GetGoods();

        Assert.Equal(new[] { apple, autoMiner }, goods); // Проверка порядка: Plant идет перед AutoMiner
    }
}
