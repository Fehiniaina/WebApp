namespace Booking.API.Extensions;

using Microsoft.AspNetCore.Diagnostics;

/// <summary>
/// Maps BookingDomainException (thrown by the aggregate, or by ValidatorBehavior wrapping a
/// FluentValidation failure) to a 400 response instead of the default 500 — eShop's
/// OrderingDomainException doesn't get this treatment in Ordering.API, but there's no reason
/// a business rule violation should surface as a server error.
/// </summary>
public sealed class BookingDomainExceptionHandler(ILogger<BookingDomainExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BookingDomainException domainException)
        {
            return false;
        }

        logger.LogWarning(domainException, "Booking domain rule violated");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Booking rule violated",
                Detail = domainException.Message
            },
            cancellationToken);

        return true;
    }
}
