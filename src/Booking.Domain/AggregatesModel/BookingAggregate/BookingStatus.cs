namespace Booking.Domain.AggregatesModel.BookingAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookingStatus
{
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Completed = 4
}
