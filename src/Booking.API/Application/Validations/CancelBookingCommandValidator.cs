namespace Booking.API.Application.Validations;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
    }
}
