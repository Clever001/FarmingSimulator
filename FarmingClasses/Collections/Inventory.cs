using FarmingClasses.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FarmingClasses.Collections;

public class Inventory<T> : ICollection<T> where T : IBuyable {
    private Dictionary<T, int> _inventory = new();

    public Inventory() { }

    public Inventory(IEnumerable<KeyValuePair<T, int>> items) {
        foreach (var kvp in items) {
            Add(kvp.Key, kvp.Value);
        }
    }

    public int Count => _inventory.Select(x => x.Value).Sum();

    public bool IsReadOnly => false;

    public int this[T good] => _inventory[good];

    public void Add(T item, int count) {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1, nameof(count));
        if (_inventory.TryGetValue(item, out int value)) _inventory[item] = value + count;
        else _inventory.Add(item, count);
    }

    public void Add(T item) {
        Add(item, 1);
    }

    public void Clear() {
        _inventory.Clear();
    }

    public bool Contains(T item) {
        if (_inventory.TryGetValue(item, out int value)) return value > 0;
        else return false;
    }

    public void CopyTo(T[] array, int arrayIndex) {
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(array));
        if (array.Length - arrayIndex < Count) throw new ArgumentException("Недостаточно места в массиве.");

        foreach (T item in this) {
            array[arrayIndex++] = item;
        }
    }

    public IEnumerator<T> GetEnumerator() {
        foreach ((T key, int value) in _inventory) {
            for (int i = value; i > 0; --i) yield return key;
        }
    }

    public IEnumerable<KeyValuePair<T, int>> GetSortedInventory() {
        return _inventory.Where(kvp => kvp.Value > 0).OrderBy(kvp => kvp.Key.Name);
    }

    public IEnumerable<T> GetSortedKeys() {
        return from kvp in _inventory
               where kvp.Value > 0
               orderby kvp.Key.Name
               select kvp.Key;
    }

    public bool Remove(T? item, int count) {
        if (item is null) return false;
        if (_inventory.TryGetValue(item, out var value)) {
            if (value >= count) {
                _inventory[item] = value - count;
                return true;
            }
            return false;
        }
        throw new ArgumentOutOfRangeException("Указанного товара не оказалось в инвентаре.");
    }

    public bool Remove(T? item) {
        return Remove(item, 1);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
