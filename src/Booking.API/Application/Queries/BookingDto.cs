namespace Booking.API.Application.Queries;

using BookingEntity = Booking.Domain.AggregatesModel.BookingAggregate.Booking;

public record BookingDto(
    Guid Id,
    Guid RoomId,
    string OrganizerId,
    string Purpose,
    DateTimeOffset Start,
    DateTimeOffset End,
    BookingStatus Status,
    int AttendeesCount,
    DateTimeOffset CreatedAt)
{
    public static BookingDto FromEntity(BookingEntity booking) => new(
        booking.Id.Value,
        booking.RoomId,
        booking.OrganizerId,
        booking.Purpose,
        booking.Slot.Start,
        booking.Slot.End,
        booking.Status,
        booking.AttendeesCount,
        booking.CreatedAt);
}
