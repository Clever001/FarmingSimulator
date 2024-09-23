using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;

/// <summary>
/// Объекты, наследующие свойства от данного интерфейса, можно будет купить или продать в магазине.
/// </summary>
public interface IBuyable : IEquatable<IBuyable> {
    /// <summary>
    /// Название товара
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Базовая стоимость товара
    /// </summary>
    public int BaseCost { get; }
    public int GetHashCode();
}