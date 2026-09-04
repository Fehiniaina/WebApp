namespace Booking.API.Application.Validations;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
    }
}
