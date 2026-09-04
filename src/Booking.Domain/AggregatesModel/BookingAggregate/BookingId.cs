namespace Booking.Domain.AggregatesModel.BookingAggregate;

public readonly record struct BookingId(Guid Value)
{
    public static BookingId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}