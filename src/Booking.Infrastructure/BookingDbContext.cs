namespace Booking.Infrastructure;

using BookingEntity = Booking.Domain.AggregatesModel.BookingAggregate.Booking;

/// <remarks>
/// Add migrations from inside the 'Booking.Infrastructure' project directory:
///
/// dotnet ef migrations add --startup-project ../Booking.API --context BookingDbContext [migration-name]
/// </remarks>
public class BookingDbContext : DbContext, IUnitOfWork
{
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

    private readonly IMediator _mediator = null!;

    // Used by EF Core design-time tooling (migrations), which has no IMediator to inject.
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }

    public BookingDbContext(DbContextOptions<BookingDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("booking");
        modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());

        // Outbox wiring (modelBuilder.UseIntegrationEventLogs()) is added once IntegrationEventLogEF
        // is pulled into the solution and Booking.API relays BookingConfirmedDomainEvent/
        // BookingCancelledDomainEvent as integration events.
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events BEFORE SaveChanges, so handler side effects join the same
        // transaction as the triggering change (see eShop's OrderingContext for the trade-offs).
        await _mediator.DispatchDomainEventsAsync(this);

        await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}
