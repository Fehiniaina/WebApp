namespace Booking.API.Application.Validations;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(c => c.RoomId).NotEmpty();
        RuleFor(c => c.OrganizerId).NotEmpty();
        RuleFor(c => c.Purpose).NotEmpty().MaximumLength(500);
        RuleFor(c => c.AttendeesCount).GreaterThan(0);

        // These duplicate TimeSlot's own invariants on purpose: this gives a fast, well-formed
        // 400 for obviously bad input before a command reaches the aggregate, which remains the
        // ultimate source of truth for the rule (see CreateBookingCommandHandler).
        RuleFor(c => c.End).GreaterThan(c => c.Start).WithMessage("End must be after Start.");
        RuleFor(c => c.Start).GreaterThanOrEqualTo(_ => DateTimeOffset.UtcNow).WithMessage("Start cannot be in the past.");
    }
}
 