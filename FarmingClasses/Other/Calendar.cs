using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;


public class Calendar {
    public DateOnly CurDay { get; private set; }

    [JsonIgnore]
    public int Year => CurDay.Year;

    [JsonIgnore]
    public int Month => CurDay.Month;

    [JsonIgnore]
    public int Day => CurDay.Day;

    public Calendar(DateOnly curDay) {
        CurDay = curDay;
    }

    public Calendar() {
        DateTime today = DateTime.Today;
        CurDay = new DateOnly(today.Year, today.Month, today.Day);
    }

    public void AddDays(int days) => CurDay = CurDay.AddDays(days);

    public void AddMonths(int months) => CurDay = CurDay.AddMonths(months);

    public override string ToString() {
        return $"Сегодняшнее число: {CurDay}.";
    }
}
