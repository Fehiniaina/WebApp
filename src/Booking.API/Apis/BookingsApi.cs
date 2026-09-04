namespace Booking.API.Apis;

public static class BookingsApi
{
    public static RouteGroupBuilder MapBookingsApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/bookings");

        api.MapPost("/", CreateBookingAsync);
        api.MapGet("/{id:guid}", GetBookingAsync);
        api.MapPut("/{id:guid}/confirm", ConfirmBookingAsync);
        api.MapPut("/{id:guid}/cancel", CancelBookingAsync);

        return api;
    }

    public static async Task<Created<Guid>> CreateBookingAsync(CreateBookingRequest request, [AsParameters] BookingServices services)
    {
        var command = new CreateBookingCommand(
            request.RoomId, request.OrganizerId, request.Purpose, request.Start, request.End, request.AttendeesCount);

        services.Logger.LogInformation("Sending command: {CommandName} ({@Command})", command.GetGenericTypeName(), command);

        var bookingId = await services.Mediator.Send(command);

        return TypedResults.Created($"/api/bookings/{bookingId}", bookingId);
    }

    public static async Task<Results<Ok<BookingDto>, NotFound>> GetBookingAsync(Guid id, [AsParameters] BookingServices services)
    {
        var booking = await services.Mediator.Send(new GetBookingByIdQuery(id));

        return booking is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(booking);
    }

    public static async Task<Results<Ok, NotFound>> ConfirmBookingAsync(Guid id, [AsParameters] BookingServices services)
    {
        var confirmed = await services.Mediator.Send(new ConfirmBookingCommand(id));

        return confirmed
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }

    public static async Task<Results<Ok, NotFound>> CancelBookingAsync(Guid id, [AsParameters] BookingServices services)
    {
        var cancelled = await services.Mediator.Send(new CancelBookingCommand(id));

        return cancelled
            ? TypedResults.Ok()
            : TypedResults.NotFound();
    }
}

public record CreateBookingRequest(
    Guid RoomId,
    string OrganizerId,
    string Purpose,
    DateTimeOffset Start,
    DateTimeOffset End,
    int AttendeesCount);
