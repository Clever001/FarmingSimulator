using System;
using FarmingClasses.Exceptions;
using Newtonsoft.Json;

namespace FarmingClasses.Other;

public readonly record struct Duration: IComparable<Duration> {
    public int Days { get; }
    public int Months { get; }

    public Duration(int days, int months = 0) {
        DurationException.CheckDuration(days, months);
        Days = days;
        Months = months;
    }

    public int CompareTo(Duration other) {
        int result = Months.CompareTo(other.Months);
        if (result == 0) result = Days.CompareTo(other.Days);
        return result;
    }
}

