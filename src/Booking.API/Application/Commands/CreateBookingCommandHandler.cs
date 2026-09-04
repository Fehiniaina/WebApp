namespace Booking.API.Application.Commands;

using BookingEntity = Booking.Domain.AggregatesModel.BookingAggregate.Booking;

public class CreateBookingCommandHandler(IBookingRepository bookingRepository, ILogger<CreateBookingCommandHandler> logger)
    : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var slot = new TimeSlot(request.Start, request.End);

        // Invariant #3 (no overlapping bookings on the same room): needs visibility over the
        // other bookings for that room, so it's checked here rather than in the Booking
        // constructor — see IBookingRepository.HasOverlapAsync.
        if (await bookingRepository.HasOverlapAsync(request.RoomId, slot, cancellationToken: cancellationToken))
        {
            throw new BookingDomainException($"Room {request.RoomId} is already booked for that time slot.");
        }

        // TODO invariant #4 (AttendeesCount <= room capacity): once Rooms.API exists, call it (or
        // a cached read model of it) here to fetch the room's capacity before creating the
        // booking — the same way eShop re-checks the catalog price when creating an order.

        var booking = new BookingEntity(request.RoomId, request.OrganizerId, request.Purpose, slot, request.AttendeesCount);

        bookingRepository.Add(booking);

        logger.LogInformation("Creating booking {BookingId} for room {RoomId}", booking.Id, booking.RoomId);

        await bookingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return booking.Id.Value;
    }
}
