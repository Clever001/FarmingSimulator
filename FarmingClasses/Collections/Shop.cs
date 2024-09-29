using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FarmingClasses.Collections;

public class Shop : IEnumerable<KeyValuePair<IBuyable, int>> {
    private const double _increase = 1.05;
    private SortedDictionary<IBuyable, int> _costs = new(Comparer<IBuyable>.Create((x, y) => {
        return (x, y) switch {
            (Plant, AutoMiner) => -1,
            (AutoMiner, Plant) => 1,
            _ => x.Name.CompareTo(y.Name),
        };
    }));

    public Shop(IEnumerable<IBuyable> goods) {
        ArgumentNullException.ThrowIfNull(goods);

        foreach (IBuyable good in goods) {
            if (!_costs.ContainsKey(good)) _costs.Add(good, good.BaseCost);
        }
    }

    public bool Buy(IBuyable good, Player player) {
        if (good is null) return false;

        if (_costs.TryGetValue(good, out int cost)) {
            if (player.PayMoney(cost)) {
                _costs[good] = (int) (cost * _increase);
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    public IEnumerable<IBuyable> GetGoods() {
        return _costs.Keys;
    }

    public IEnumerator<KeyValuePair<IBuyable, int>> GetEnumerator() {
        return _costs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
