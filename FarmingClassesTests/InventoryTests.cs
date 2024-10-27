using System;
using System.Collections.Generic;
using Xunit;
using FarmingClasses.Collections;
using FarmingClasses.Other;

namespace FarmingClassesTests;

public class InventoryTests {
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
            return obj is MockBuyable other && Name.Equals(other.Name);
        }

        public bool Equals(IBuyable? other) {
            return other is not null && Name.Equals(other.Name);
        }
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyInventory() {
        var inventory = new Inventory<MockBuyable>();

        Assert.Empty(inventory);
    }

    [Fact]
    public void Constructor_WithItems_ShouldAddItemsToInventory() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var items = new List<KeyValuePair<MockBuyable, int>> {
            new KeyValuePair<MockBuyable, int>(apple, 2),
            new KeyValuePair<MockBuyable, int>(banana, 3)
        };

        var inventory = new Inventory<MockBuyable>(items);

        Assert.Equal(5, inventory.Count);
        Assert.Equal(2, inventory[apple]);
        Assert.Equal(3, inventory[banana]);
    }

    [Fact]
    public void Add_ShouldIncreaseItemCount_WhenItemAlreadyExists() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable> {
            { apple, 3 },
            { apple, 2 }
        };

        Assert.Equal(5, inventory[apple]);
    }

    [Fact]
    public void Add_ShouldThrow_WhenCountIsLessThanOne() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable>();

        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Add(apple, 0));
    }

    [Fact]
    public void Contains_ShouldReturnTrue_WhenItemExistsWithCountGreaterThanZero() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable> {
            { apple, 1 }
        };

#pragma warning disable
        Assert.True(inventory.Contains(apple));
#pragma warning restore
    }

    [Fact]
    public void Contains_ShouldReturnFalse_WhenItemDoesNotExist() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable>();

#pragma warning disable
        Assert.False(inventory.Contains(apple));
#pragma warning restore
    }

    [Fact]
    public void Remove_ShouldDecreaseItemCount_WhenItemExistsAndCountIsEnough() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable> {
            { apple, 3 }
        };

        var result = inventory.Remove(apple, 2);

        Assert.True(result);
        Assert.Equal(1, inventory[apple]);
    }

    [Fact]
    public void Remove_ShouldReturnFalse_WhenItemCountIsNotEnough() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable> {
            { apple, 1 }
        };

        var result = inventory.Remove(apple, 2);

        Assert.False(result);
        Assert.Equal(1, inventory[apple]);
    }

    [Fact]
    public void Remove_ShouldThrow_WhenItemNotInInventory() {
        var apple = new MockBuyable("Apple", 10);
        var inventory = new Inventory<MockBuyable>();

        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Remove(apple, 1));
    }

    [Fact]
    public void Clear_ShouldRemoveAllItems() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var inventory = new Inventory<MockBuyable> {
            { apple, 2 },
            { banana, 3 }
        };

        inventory.Clear();

        Assert.Empty(inventory);
    }

    [Fact]
    public void GetSortedInventory_ShouldReturnItemsSortedByName() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var carrot = new MockBuyable("Carrot", 5);
        var inventory = new Inventory<MockBuyable> {
            { carrot, 2 },
            { apple, 1 },
            { banana, 3 }
        };

        var sortedInventory = inventory.GetSortedInventory();

        Assert.Equal(new[] { apple, banana, carrot }, sortedInventory.Select(kvp => kvp.Key).ToArray());
    }

    [Fact]
    public void CopyTo_ShouldCopyItemsToArray() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var inventory = new Inventory<MockBuyable> {
            { apple, 1 },
            { banana, 1 }
        };

        var array = new MockBuyable[2];
        inventory.CopyTo(array, 0);

        Assert.Contains(apple, array);
        Assert.Contains(banana, array);
    }

    [Fact]
    public void Enumerator_ShouldIterateOverAllItemsWithCounts() {
        var apple = new MockBuyable("Apple", 10);
        var banana = new MockBuyable("Banana", 15);
        var inventory = new Inventory<MockBuyable> {
            { apple, 2 },
            { banana, 1 }
        };

        var items = new List<MockBuyable>(inventory);

        Assert.Equal(3, items.Count);
        Assert.Equal(2, items.Count(i => i == apple));
        Assert.Equal(1, items.Count(i => i == banana));
    }
}
