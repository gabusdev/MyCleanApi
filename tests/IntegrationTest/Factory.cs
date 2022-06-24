using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest;
public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryEmployeeTest");
            });
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }
        });
    }
}
public class CustomRemoteIpAddressMiddleware : IMiddleware
{

    private readonly IPAddress _fakeIpAddress;

    public CustomRemoteIpAddressMiddleware(IPAddress? fakeIpAddress = null)
    {
        _fakeIpAddress = fakeIpAddress ?? IPAddress.Parse("127.0.0.1");
    }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Connection.RemoteIpAddress = _fakeIpAddress;
        await next(context);
        context.Response.StatusCode = 465;
    }
}
public class CustomRemoteIpStartupFilter : IStartupFilter
{
    private readonly IPAddress? _remoteIp;

    public CustomRemoteIpStartupFilter(IPAddress? remoteIp = null)
    {
        _remoteIp = remoteIp;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.UseMiddleware<CustomRemoteIpAddressMiddleware>(_remoteIp);
            next(app);
        };
    }
}
