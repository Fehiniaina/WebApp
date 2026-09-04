using System.ComponentModel.DataAnnotations;
using Booking.Domain.Exceptions;

namespace Booking.Domain.AggregatesModel.BookingAggregate;

public class Booking : Entity, IAggregateRoot
{
    public BookingId Id { get; private set; }

    public Guid RoomId { get; private set;}

    public string OrganizerId { get; private set; } = default!;
    public string Purpose { get; private set; } = default!;
    public TimeSlot Slot { get; private set; } = default!;
    public BookingStatus Status { get; private set; }
    public int AttendeesCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Booking() {}

    public Booking(Guid roomId, string organizerId, string purpose, TimeSlot slot, int attendeesCount)
    {
        if (roomId == Guid.Empty)
        {
            throw new BookingDomainException("A booking must reference a room.");
        }

        if (string.IsNullOrWhiteSpace(organizerId))
        {
            throw new BookingDomainException("A booking must have an organizer.");
        }

        if (string.IsNullOrWhiteSpace(purpose))
        {
            throw new BookingDomainException("A booking must state its purpose.");
        }

        if (attendeesCount <= 0)
        {
            throw new BookingDomainException("A booking must have at least one attendee.");
        }

        Id = BookingId.New();
        RoomId = roomId;
        OrganizerId = organizerId;
        Purpose = purpose;
        Slot = slot ?? throw new ArgumentNullException(nameof(slot));
        AttendeesCount = attendeesCount;
        CreatedAt = DateTimeOffset.UtcNow;

        AddDomainEvent(new BookingConfirmedDomainEvent(this));
    }

    // DDD Patterns comment
    // Status transitions are only ever performed through these methods, never through a public
    // setter, so invariants (valid transitions) always hold and every transition raises the
    // matching domain event. Overlap-with-other-bookings and room-capacity checks are NOT here:
    // they need to look across other aggregates/bounded contexts, so they live in the repository
    // (overlap) and in the application handler (capacity), which call Confirm()/Cancel() only
    // once those checks have passed.

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
        {
            throw new BookingDomainException($"Cannot confirm a booking that is {Status}.");
        }

        Status = BookingStatus.Confirmed;
        AddDomainEvent(new BookingConfirmedDomainEvent(this));
    }

    public void Cancel()
    {
        if (Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            throw new BookingDomainException($"Cannot cancel a booking that is {Status}.");
        }

        Status = BookingStatus.Cancelled;
        AddDomainEvent(new BookingCancelledDomainEvent(this));
    }

    public void Complete()
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new BookingDomainException($"Cannot complete a booking that is {Status}.");
        }

        Status = BookingStatus.Completed;
    }

    public void Reschedule(TimeSlot newSlot)
    {
        if (Status is BookingStatus.Cancelled or BookingStatus.Completed)
        {
            throw new BookingDomainException($"Cannot reschedule a booking that is {Status}.");
        }

        Slot = newSlot ?? throw new ArgumentNullException(nameof(newSlot));
    }
}