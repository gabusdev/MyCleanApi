using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Test;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder) =>
    hostBuilder
        .ConfigureHostConfiguration(builder => { })
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        });

    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        var c = context.Configuration;
        services.AddInfrastructure(c);
    }
}
