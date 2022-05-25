using GraphQL.Queries;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL
{
    public static class Startup
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddAuthorization()
                .RegisterService<IMediator>(ServiceKind.Synchronized)
                .AddQueryType<QueryType>()
                .AddTypeExtension<ProductExtensions>()
                .AddTypeExtension<UserExtension>();
            
            return services;
        }
    }
}