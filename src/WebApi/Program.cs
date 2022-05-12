using Infrastructure.Logging;
using Infrastructure.Persistence.Initialization;
using Serilog;
using WebApi;

Log.Logger = LoggingExtension.ConfigureLogger();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddConfigurations(builder.Configuration);

var app = builder.Build();
// Use services in the container.
app.UseConfigurations(builder.Configuration);
// Create and Seed Database
await app.InitializeAndSeedDatabaseAsync();

try
{
    Log.Information("Application is Starting...");
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