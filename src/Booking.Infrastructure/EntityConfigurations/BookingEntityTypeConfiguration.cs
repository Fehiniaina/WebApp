namespace Booking.Infrastructure.EntityConfigurations;

using BookingEntity = Booking.Domain.AggregatesModel.BookingAggregate.Booking;

class BookingEntityTypeConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("bookings");

        builder.Ignore(b => b.DomainEvents);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(id => id.Value, value => new BookingId(value))
            .ValueGeneratedNever();

        builder.Property(b => b.RoomId)
            .IsRequired();

        builder.Property(b => b.OrganizerId)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(b => b.Purpose)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(b => b.AttendeesCount)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        // TimeSlot is a value object persisted as an EF Core owned entity.
        builder.OwnsOne(b => b.Slot, slot =>
        {
            slot.Property(s => s.Start).HasColumnName("SlotStart").IsRequired();
            slot.Property(s => s.End).HasColumnName("SlotEnd").IsRequired();
        });

        builder.Navigation(b => b.Slot).IsRequired();

        // Speeds up IBookingRepository.HasOverlapAsync; it does not enforce the invariant itself —
        // that's still the repository query plus the aggregate's own guard clauses.
        builder.HasIndex(b => b.RoomId);
    }
}
