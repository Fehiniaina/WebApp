namespace Booking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private static readonly BookingStatus[] ActiveStatuses = [BookingStatus.Pending, BookingStatus.Confirmed];

    private readonly BookingDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public BookingEntity Add(BookingEntity booking) => _context.Bookings.Add(booking).Entity;

    public void Update(BookingEntity booking) => _context.Entry(booking).State = EntityState.Modified;

    public async Task<BookingEntity?> GetAsync(BookingId id) => await _context.Bookings.FindAsync(id);

    public async Task<bool> HasOverlapAsync(
        Guid roomId,
        TimeSlot slot,
        BookingId? excludingBookingId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings.Where(b =>
            b.RoomId == roomId &&
            ActiveStatuses.Contains(b.Status) &&
            b.Slot.Start < slot.End &&
            slot.Start < b.Slot.End);

        if (excludingBookingId is { } id)
        {
            query = query.Where(b => b.Id != id);
        }

        // Cancellation token permet d'annuler la requete DB si le client HTTP annule sa requete (timeout, navigation ailleurs)
        // sans attendre inutilement une reponse SQL.
        return await query.AnyAsync(cancellationToken);
    }
}