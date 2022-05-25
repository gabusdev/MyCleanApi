using GraphQL.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL
{
    public static class Startup
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query2>();
            
            return services;
        }
    }
}