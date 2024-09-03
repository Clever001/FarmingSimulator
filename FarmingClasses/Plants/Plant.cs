using FarmingClasses.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Plants;

public interface IPlant {
    public string Name { get; }

    public DateOnly Planted { get; }

    public Duration MaturationTime { get; }

    public bool IsReady(DateOnly date) {
        DateOnly endDay = Planted.AddDays(MaturationTime.Days).AddMonths(MaturationTime.Months);
        throw new NotImplementedException();
    }

    public string Description { get; }
}