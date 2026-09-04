namespace Booking.Domain.Events;

public sealed record BookingRequestedDomainEvent(Booking.Domain.AggregatesModel.BookingAggregate.Booking Booking) : INotification;