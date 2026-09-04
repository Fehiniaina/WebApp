namespace Booking.API.Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .ToList();

        if (failures.Count > 0)
        {
            logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            // Wrapped in a BookingDomainException so BookingDomainExceptionHandler turns it into a
            // 400 response, same as an invariant violation raised by the aggregate itself.
            throw new BookingDomainException(
                $"Command validation errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
        }

        return await next();
    }
}
