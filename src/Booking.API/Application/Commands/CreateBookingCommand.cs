namespace Booking.API.Application.Commands;

public record CreateBookingCommand(
    Guid RoomId,
    string OrganizerId,
    string Purpose,
    DateTimeOffset Start,
    DateTimeOffset End,
    int AttendeesCount) : IRequest<Guid>;
