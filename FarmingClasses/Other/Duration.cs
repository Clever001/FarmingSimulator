using System;
using FarmingClasses.Exceptions;

namespace FarmingClasses.Other;

public record struct Duration: IComparable<Duration> {
    public int Days { get; init; }
    public int Months { get; init; }

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

