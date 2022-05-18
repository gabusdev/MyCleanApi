using Application.Common.Interfaces;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundJobs;

internal static class Startup
{
    internal static IServiceCollection AddBackgroundJobs (this IServiceCollection services, IConfiguration config)
    {
        var settings = HangfireSettings.GetHangfireSettings(config);
        
        services.AddTransient<IJobService, HangfireService>();
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(settings.Constring, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        return services;
    }
    internal static IApplicationBuilder UseBackgroundJobs(this IApplicationBuilder app, IConfiguration configuration)
    {
        var settings = HangfireSettings.GetHangfireSettings(configuration);

        DashboardOptions dashboardOptions = new() 
        {
            DashboardTitle = "Jobs",
            StatsPollingInterval = 2000,
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = settings.User,
                    Pass = settings.Password
                }
            }
        };
        return app.UseHangfireDashboard("/jobs", dashboardOptions);
    }
}
