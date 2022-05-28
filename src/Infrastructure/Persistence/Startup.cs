using Application.Common.Persistence;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Initialization;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    internal static class Startup
    {
        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            // Add Initialize and Seed Database Classes
            services
                .AddTransient<IApplicationDbInitializer, ApplicationDbInitializer>()
                .AddTransient<IApplicationDbSeeder, ApplicationDbSeeder>();

            // Add Database Context form Enviroment or Configuration files
            var dBSettings = DatabaseSettings.GetDbSettings(config);
            services.AddDbContext<ApplicationDbContext>(m =>
                m.UseDatabase(dBSettings.DBProvider!, dBSettings.ConnectionString!));

            // Adding Support for Dapper with same Connection String
            services
                .AddSingleton<DapperContext>()
                .AddScoped<IDapperService, DapperService>()
                .AddTransient<IUnitOfWork, UnitOfWork>();

            Log.Information($"Using Database Context: {dBSettings.DBProvider} with connString: {dBSettings.ConnectionString}");

            return services;
        }

        private static (DbContextOptionsBuilder, ServiceLifetime) UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            var defaultLifetime = ServiceLifetime.Scoped;
            switch (dbProvider.ToLowerInvariant())
            {

                case DbProviderKeys.Npgsql:
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    return (builder.UseNpgsql(connectionString), defaultLifetime);
                case DbProviderKeys.SqlServer:
                    return (builder.UseSqlServer(connectionString), defaultLifetime);
                case DbProviderKeys.MySql:
                    return (builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)), defaultLifetime);
                case DbProviderKeys.Oracle:
                    return (builder.UseOracle(connectionString), defaultLifetime);
                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }

    }
}