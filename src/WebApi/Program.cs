using WebApi;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddConfigurations(builder.Configuration);


var app = builder.Build();
// Use services in the container.
app.UseConfigurations(builder.Configuration);

app.Run();
