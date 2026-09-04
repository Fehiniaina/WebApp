namespace Booking.Domain.Exceptions;

/// <summary>
/// Exception type for domain invariant violations (thrown by the Booking aggregate itself).
/// </summary>
public class BookingDomainException : Exception
{
    public BookingDomainException()
    { }

    public BookingDomainException(string message)
        : base(message)
    { }

    public BookingDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
