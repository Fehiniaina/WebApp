namespace Booking.API.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        // Pooling is disabled: BookingDbContext has more than one constructor (design-time vs.
        // DI+IMediator), same reason Ordering.API disables it for OrderingContext.
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("bookingdb"));
        });
        builder.EnrichNpgsqlDbContext<BookingDbContext>();

        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<CreateBookingCommandValidator>();

        services.AddExceptionHandler<BookingDomainExceptionHandler>();
        services.AddProblemDetails();
    }
}
