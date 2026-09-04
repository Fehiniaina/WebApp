namespace Booking.Domain.Events;

public sealed record BookingCancelledDomainEvent(Booking.Domain.AggregatesModel.BookingAggregate.Booking Booking) : INotification;