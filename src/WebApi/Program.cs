using Infrastructure.Logging;
using Infrastructure.Persistence.Initialization;
using Serilog;
using WebApi;

Log.Logger = LoggingExtension.ConfigureLogger();

Log.Information("Application is Starting...");

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddConfigurations(builder.Configuration);

var app = builder.Build();
// Use services in the container.
app.UseConfigurations(builder.Configuration, app.Environment.IsDevelopment());
// Create and Seed Database
await app.InitializeAndSeedDatabaseAsync();
// Add GraphQL Interface
app.MapGraphQL();

try
{
    Log.Information("Application is Running...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplication Failed to Start");
}
finally
{
    Log.Warning("Application is Shuting Down...");
    Log.CloseAndFlush();
}