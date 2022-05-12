using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    internal static class Startup
    {
        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var dBSettings = GetDbSettings(config);
            
            services.AddDbContext<ApplicationDbContext>(m =>
                m.UseDatabase(dBSettings.DBProvider!, dBSettings.ConnectionString!));
            
            Log.Information($"Using Database Context: {dBSettings.DBProvider} with connString: {dBSettings.ConnectionString}");
            
            return services;
        }

        private static DatabaseSettings GetDbSettings(IConfiguration config)
        {
            // First get Settings from Configuration file
            var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            // The check if there are Settings in Enviroment
            GetSettingsFromEnv(databaseSettings);

            if (databaseSettings.DBProvider == null)
            {
                databaseSettings.DBProvider = "InMemory";
            }
            else if (databaseSettings.DBProvider.ToLower() != "inmemory" 
                && databaseSettings.ConnectionString is null)
            {
                throw new InvalidOperationException($"Connection String not provided for {databaseSettings.DBProvider}");
            }
            return databaseSettings;
        }
        private static void GetSettingsFromEnv(DatabaseSettings dBSettings)
        {
            var dbProvider = Environment.GetEnvironmentVariable("dbProvider");
            if (dbProvider != null)
                dBSettings.DBProvider = dbProvider;

            var connString = Environment.GetEnvironmentVariable("connString");
            if (connString != null)
                dBSettings.ConnectionString = connString;
        }
        private static (DbContextOptionsBuilder, ServiceLifetime) UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            var defaultLifetime = ServiceLifetime.Scoped;
            switch (dbProvider.ToLowerInvariant())
            {
                /*
                case DbProviderKeys.Npgsql:
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    return builder.UseNpgsql(connectionString, e =>
                         e.MigrationsAssembly("Migrators.PostgreSQL"));
                */
                case DbProviderKeys.SqlServer:
                    return (builder.UseSqlServer(connectionString, e =>
                         e.MigrationsAssembly("Migrators.MSSQL")),defaultLifetime);
                /*
                case DbProviderKeys.MySql:
                    return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), e =>
                         e.MigrationsAssembly("Migrators.MySQL")
                          .SchemaBehavior(MySqlSchemaBehavior.Ignore));
                */
                /*
                case DbProviderKeys.Oracle:
                    return builder.UseOracle(connectionString, e =>
                         e.MigrationsAssembly("Migrators.Oracle"));
                */
                case DbProviderKeys.InMemory:
                    return (builder.UseInMemoryDatabase(Guid.NewGuid().ToString()),ServiceLifetime.Singleton);
                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }
        
    }
}