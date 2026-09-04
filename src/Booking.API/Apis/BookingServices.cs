namespace Booking.API.Apis;

public class BookingServices(IMediator mediator, ILogger<BookingServices> logger)
{
    public IMediator Mediator { get; } = mediator;

    public ILogger<BookingServices> Logger { get; } = logger;
}
