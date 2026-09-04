namespace Booking.Domain.AggregatesModel.BookingAggregate;

public interface IBookingRepository : IRepository<Booking>
{
    Booking Add(Booking booking);

    void Update(Booking booking);

    Task<Booking?> GetAsync(BookingId id);

    Task<bool> HasOverlapAsync(
        Guid roomId,
        TimeSlot slot,
        BookingId? excludingBookingId = null,
        CancellationToken cancellationToken = default);
}