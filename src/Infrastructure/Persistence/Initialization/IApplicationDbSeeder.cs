using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Initialization
{
    internal interface IApplicationDbSeeder
    {
        Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken);
    }
}
