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
                //.AddMutationConventions(applyToAllMutations: true)
                .RegisterService<IMediator>(ServiceKind.Synchronized)
                .AddQueryType()
                .AddMutationType()
                .AddTypeExtension<UserQueries>()
                .AddTypeExtension<UserExtension>()
                .AddTypeExtension<UserMutations>()
                ;
            
            return services;
        }
    }
}