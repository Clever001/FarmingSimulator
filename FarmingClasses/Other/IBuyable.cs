using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;

/// <summary>
/// Объекты, наследующие свойства от данного интерфейса, можно будет купить или продать в магазине.
/// </summary>
public interface IBuyable {
    public string Name { get; }
    public int BaseCost { get; }
}
