namespace Booking.API.Application.Commands;

public record CancelBookingCommand(Guid BookingId) : IRequest<bool>;
