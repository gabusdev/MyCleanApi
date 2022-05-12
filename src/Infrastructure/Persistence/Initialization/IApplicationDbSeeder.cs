using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Initialization
{
    internal interface IApplicationDbSeeder
    {
        Task SeedDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken);
    }
}
