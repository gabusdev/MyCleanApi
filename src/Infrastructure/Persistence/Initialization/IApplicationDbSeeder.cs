using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initialization
{
    internal interface IApplicationDbSeeder
    {
        Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken);
    }
}
