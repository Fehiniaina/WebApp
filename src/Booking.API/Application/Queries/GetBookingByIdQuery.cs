namespace Booking.API.Application.Queries;

public record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingDto?>;
