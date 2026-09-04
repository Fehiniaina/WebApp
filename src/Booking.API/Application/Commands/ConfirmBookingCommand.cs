namespace Booking.API.Application.Commands;

public record ConfirmBookingCommand(Guid BookingId) : IRequest<bool>;
