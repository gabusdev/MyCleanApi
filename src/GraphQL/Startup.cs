using GraphQL.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL
{
    public static class Startup
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            services.AddGraphQLServer().AddQueryType<Query>();
            
            return services;
        }
    }
}