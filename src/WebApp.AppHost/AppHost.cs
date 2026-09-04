var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var bookingDb = postgres.AddDatabase("bookingdb");

var apiService = builder.AddProject<Projects.WebApp_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Booking_API>("booking-api")
    .WithReference(bookingDb)
    .WaitFor(bookingDb)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.WebApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
