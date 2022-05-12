using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Initialization
{
    public static class Startup
    {
        public static async Task InitializeAndSeedDatabaseAsync(this IApplicationBuilder app, CancellationToken cancellationToken = default)
        {
            // Create a new scope to retrieve scoped services
            using var scope = app.ApplicationServices.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IApplicationDbInitializer>()
                .InitializeDatabaseAsync(cancellationToken);
        }
    }
}
