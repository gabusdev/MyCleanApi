using Application.Common.Events;
using GraphQL.Mutations;
using GraphQL.Queries;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Services;
using GraphQL.Subscriptions;
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
                .AddQueryType()
                .AddMutationType()
                .AddSubscriptionType()

                .AddAuthorization()
                .AddInMemorySubscriptions()
                .AddMutationConventions()
                .AddFiltering()
                .AddSorting()
                .AddProjections()

                .RegisterService<IMediator>(ServiceKind.Synchronized)

                .AddTypeExtension<UserQueries>()
                .AddTypeExtension<UserExtension>()
                .AddTypeExtension<UserMutations>()
                .AddTypeExtension<UserSubscriptions>()
                ;

            services.AddScoped<IGraphQLSubscriptionService, GraphQLSubscriptionService>();

            return services;
        }

        public static IApplicationBuilder UseMyGraphQL(this IApplicationBuilder app)
        {
            app.UseWebSockets();

            return app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql"
            }, "/graphql-voyager");
        }
    }
}