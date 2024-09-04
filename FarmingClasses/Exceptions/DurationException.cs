using System;

namespace FarmingClasses.Exceptions;
public class DurationException(int days, int months)
    : ArgumentException("The elements of the duration time cannot be negative or together equal to zero.") {
    public int Days { get; } = days;
    public int Months { get; } = months;

    public static void CheckDuration(int days, int months) {
        if (days < 0 || months < 0 || (days == 0 && months == 0))
            throw new DurationException(days, months);
    }
}

