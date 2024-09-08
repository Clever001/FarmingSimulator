using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;

internal class NameComparer : IEqualityComparer<IBuyable> {
    public bool Equals(IBuyable? x, IBuyable? y) {
        if (x is null || y is null) return false;
        return x.Name == y.Name;
    }

    public int GetHashCode([DisallowNull] IBuyable obj) {
        return obj.Name.GetHashCode();
    }
}
