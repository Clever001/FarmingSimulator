using FarmingClasses.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;

namespace FarmingClasses.Collections;

public class Inventory : ICollection<IBuyable> {
    private Dictionary<IBuyable, int> _inventory = new();

    public int Count => _inventory.Count;

    public bool IsReadOnly => false;

    public void Add(IBuyable item, int count = 1) {
        if (_inventory.TryGetValue(item, out int value)) _inventory[item] = value + count;
        else _inventory.Add(item, 1);
    }

    public void Add(IBuyable item) {
        Add(item, 1);
    }

    public void Clear() {
        _inventory.Clear();
    }

    public bool Contains(IBuyable item) {
        if (_inventory.TryGetValue(item, out int value)) return value > 0;
        else return false;
    }

    public void CopyTo(IBuyable[] array, int arrayIndex) {
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(array));
        if (array.Length - arrayIndex < Count) throw new ArgumentException("Недостаточно места в массиве.");

        foreach (IBuyable item in this) {
            array[arrayIndex++] = item;
        }
    }

    public IEnumerator<IBuyable> GetEnumerator() {
        foreach ((IBuyable key, int value) in _inventory) {
            for (int i = value; i > 0; --i) yield return key;
        }
    }

    public IEnumerable<KeyValuePair<IBuyable, int>> GetSortedInventory() {
        return from keyVal in _inventory
               orderby keyVal.Key.Name
               select keyVal;
    }

    public bool Remove(IBuyable item, int count = 1) {
        if (_inventory.TryGetValue(item, out var value)) {
            if (value >= count) {
                _inventory[item] = value - count;
                return true;
            }
            return false;
        } 
        return false;
    }

    public bool Remove(IBuyable item) {
        return Remove(item, 1);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
