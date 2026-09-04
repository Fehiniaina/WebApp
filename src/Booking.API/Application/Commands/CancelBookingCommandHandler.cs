namespace Booking.API.Application.Commands;

public class CancelBookingCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<CancelBookingCommand, bool>
{
    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(new BookingId(request.BookingId));
        if (booking is null)
        {
            return false;
        }

        booking.Cancel();

        return await bookingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
