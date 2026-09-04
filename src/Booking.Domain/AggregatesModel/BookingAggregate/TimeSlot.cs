namespace Booking.Domain.AggregatesModel.BookingAggregate;

public sealed record TimeSlot
{
    public static readonly TimeSpan MinimumDuration = TimeSpan.FromMinutes(15);

    public DateTimeOffset Start { get; private set;  }

    public DateTimeOffset End { get; private set;  }

    public TimeSpan Duration => End - Start;

    private TimeSlot() {}

    public TimeSlot(DateTimeOffset start, DateTimeOffset end)
    {
        if (end <= start)
        {
            throw new BookingDomainException("A time slot's end must be strictly after its start.");
        }

        if (end - start < MinimumDuration)
        {
            throw new BookingDomainException($"A time slot must last at least {MinimumDuration.TotalMinutes} minutes.");
        }

        if (start < DateTimeOffset.UtcNow)
        {
            throw new BookingDomainException("A time slot cannot start in the past.");
        }

        Start = start;
        End = end;
    }

    /// <summary>
    /// True when this slot and <paramref name="other"/> share any instant in time.
    /// Back-to-back slots (this.End == other.Start) do not overlap.
    /// </summary>
    public bool Overlaps(TimeSlot other) => Start < other.End && other.Start < End;
}
