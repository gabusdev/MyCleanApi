using Application.Common.Interfaces;
using Hangfire;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Infrastructure.Persistence;

namespace Infrastructure.BackgroundJobs;

internal static class Startup
{
    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration config)
    {
        var settings = HangfireSettings.GetHangfireSettings(config);

        services.AddTransient<IJobService, HangfireService>();
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseDatabase(settings.Provider, settings.Constring));

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
    private static IGlobalConfiguration UseDatabase(this IGlobalConfiguration hangfireConfig, string? dbProvider, string? connectionString)
    {
        if (dbProvider is null || connectionString is null)
            throw new ArgumentNullException("Either Provider or Connection String for HangFire are null.");

        var sqlOptions = new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        };

        var pgsqlOptions = new PostgreSqlStorageOptions
        {
            QueuePollInterval = TimeSpan.FromSeconds(10),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            PrepareSchemaIfNecessary = true
        };

        var myqslOptions = new MySqlStorageOptions
        {
            QueuePollInterval = TimeSpan.FromSeconds(10),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 25000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "Hangfire",
        };

        return dbProvider.ToLowerInvariant() switch
        {
            DbProviderKeys.Npgsql =>
                hangfireConfig.UsePostgreSqlStorage(connectionString, pgsqlOptions),
            DbProviderKeys.SqlServer =>
                hangfireConfig.UseSqlServerStorage(connectionString, sqlOptions),
            DbProviderKeys.MySql =>
                hangfireConfig.UseStorage(new MySqlStorage(connectionString, myqslOptions)),
            _ => throw new Exception($"Hangfire Storage Provider {dbProvider} is not supported.")
        };
    }

}
