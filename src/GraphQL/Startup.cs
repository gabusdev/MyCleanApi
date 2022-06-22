#region Includes
using Application.Common.Events;
using GraphQL.Endpoints.Mutations;
using GraphQL.Endpoints.Queries;
using GraphQL.Endpoints.Subscriptions;
using GraphQL.ErrorFilters;
using GraphQL.Interceptors;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
#endregion

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
                .AddErrorFilter<GQLErrorFilter>()

                .RegisterService<IMediator>(ServiceKind.Synchronized)

                .AddSocketSessionInterceptor<SocketSubscriptionInterceptor>()
                .AddHttpRequestInterceptor<QueryLoggerInterceptor>()

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
                GraphQLEndPoint = "/api/graphql"
            }, "/dev/graphql-voyager");
        }
    }
}