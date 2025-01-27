using Scalar.AspNetCore;
using TheMover.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Creates API Specification
    app.MapOpenApi();
    
    // Consumes the spec and generates a web frontend
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("TheMover API")
            .WithTheme(ScalarTheme.Moon)
            .WithDarkMode(darkMode: true)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
    
    // Add grpc reflection for local development
    app.MapGrpcReflectionService();
}

// Create gRPC Endpoints
app.MapGrpcService<GreeterService>()
    .RequireHost("*:5001");

CreateRestEndpoints(app);

app.Run();

return;

static void CreateRestEndpoints(WebApplication app)
{
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909")
        .WithName("TestEndpoint")
        .RequireHost("*:5000");
}
