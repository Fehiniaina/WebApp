namespace Booking.Domain.Events;

public sealed record BookingConfirmedDomainEvent(Booking.Domain.AggregatesModel.BookingAggregate.Booking Booking) : INotification;