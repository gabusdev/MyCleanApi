using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initialization
{
    internal interface IApplicationDbSeeder
    {
        Task SeedDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken);
    }
}
