namespace Booking.API.Application.Queries;

// Reads go through IBookingRepository rather than a separate Dapper-backed query service (as
// Ordering.API's IOrderQueries does): the Booking aggregate has no joins/read-model shaping
// complex enough yet to justify that split. Revisit if list/filter endpoints need it later.
public class GetBookingByIdQueryHandler(IBookingRepository bookingRepository) : IRequestHandler<GetBookingByIdQuery, BookingDto?>
{
    public async Task<BookingDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(new BookingId(request.BookingId));
        return booking is null ? null : BookingDto.FromEntity(booking);
    }
}
