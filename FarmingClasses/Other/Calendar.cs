using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FarmingClasses.Other;


public class Calendar {
    [XmlIgnore]
    public DateOnly CurDay { get; set; }

    [JsonIgnore]
    public string DateString {
        get { return this.CurDay.ToString("yyyy-MM-dd"); }
        set { this.CurDay = DateOnly.Parse(value); }
    }

    [XmlIgnore]
    [JsonIgnore]
    public int Year => CurDay.Year;

    [XmlIgnore]
    [JsonIgnore]
    public int Month => CurDay.Month;

    [XmlIgnore]
    [JsonIgnore]
    public int Day => CurDay.Day;

    [JsonConstructor]
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
