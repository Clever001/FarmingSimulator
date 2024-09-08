using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;

internal class NameComparer : IEqualityComparer<IBuyable> {
    public bool Equals(IBuyable? x, IBuyable? y) {
        throw new NotImplementedException();
    }

    public int GetHashCode([DisallowNull] IBuyable obj) {
        throw new NotImplementedException();
    }
}
