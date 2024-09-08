using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Other;
public class Calendar {
    public DateOnly CurDay { get; private set; }

    public Calendar(DateOnly? day = null) {
        if (day is null) {
            DateTime today = DateTime.Today;
            CurDay = new(today.Year, today.Month, today.Day);
        } else {
            CurDay = day.Value;
        }
    }

    public void AddDays(int days) => CurDay = CurDay.AddDays(days);

    public void AddMonths(int months) => CurDay = CurDay.AddMonths(months);

    public override string ToString() {
        return $"Сегодняшнее число: {CurDay}.";
    }
}
