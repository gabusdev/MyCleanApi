using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initialization
{
    internal class ApplicationDbInitializer : IApplicationDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IApplicationDbSeeder _dbSeeder;

        public ApplicationDbInitializer(ApplicationDbContext dbContext, IApplicationDbSeeder dbSeeder)
        {
            _dbContext = dbContext;
            _dbSeeder = dbSeeder;
        }

        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            if (_dbContext.Database.IsInMemory())
            {
                await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
                Log.Information("Using In Memory database");
                return;
            }

            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    Log.Information("Applying Migrations in Database.");
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    Log.Information("Connection to Database Succeeded.");

                    await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
                }
            }
        }
    }
}
