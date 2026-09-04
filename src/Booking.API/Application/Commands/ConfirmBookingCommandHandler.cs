namespace Booking.API.Application.Commands;

public class ConfirmBookingCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<ConfirmBookingCommand, bool>
{
    public async Task<bool> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(new BookingId(request.BookingId));
        if (booking is null)
        {
            return false;
        }

        booking.Confirm();

        return await bookingRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}
