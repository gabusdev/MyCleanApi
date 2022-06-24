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
            Console.WriteLine("Helooooooo");
            Log.Information("Adding Test Filter");
            services.AddScoped<CustomRemoteIpAddressMiddleware>();
            services.AddScoped<IpChecker>();
            services.AddSingleton<IStartupFilter>(new CustomRemoteIpStartupFilter(IPAddress.Parse("127.0.0.1")));

            var sp = services.BuildServiceProvider();
        });
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
        }
    }
    public class IpChecker
    {
        private readonly RequestDelegate _next;

        public IpChecker(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Log.Information($"Ip before:{httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()}");
            await _next(httpContext);
        }
    }

    public class CustomRemoteIpStartupFilter : IStartupFilter
    {
        private readonly IPAddress _remoteIp;

        public CustomRemoteIpStartupFilter(IPAddress remoteIp)
        {
            _remoteIp = remoteIp;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<CustomRemoteIpAddressMiddleware>(_remoteIp);
                app.UseMiddleware<IpChecker>(_remoteIp);
                next(app);
            };
        }
    }
}
