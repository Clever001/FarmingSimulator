using FarmingClasses.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClassesTests;

public class CalendarTests {
    [Fact]
    public void TestCalendar() {
        DateOnly date = new(2000, 12, 13);
        Calendar c = new(date);

        Assert.Equal(date, c.CurDay);

        date = date.AddDays(95);
        c.AddDays(95);
        Assert.Equal(date, c.CurDay);

        date = date.AddMonths(14);
        c.AddMonths(14);
        Assert.Equal(date, c.CurDay);
    }
}
