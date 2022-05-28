using GraphQL.Queries;
using GraphQL.Server.Ui.Voyager;
using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
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
                .AddMutationConventions()
                .RegisterService<IMediator>(ServiceKind.Synchronized)
                .AddQueryType()
                .AddMutationType()
                .AddTypeExtension<UserQueries>()
                .AddTypeExtension<UserExtension>()
                .AddTypeExtension<UserMutations>()
                ;
            
            return services;
        }

        public static IApplicationBuilder UseMyGraphQL(this IApplicationBuilder app)
        {
            return app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            }, "/graphql-voyager");
        }
    }
}