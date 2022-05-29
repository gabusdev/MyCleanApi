using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetUserPermissions;
using Application.Identity.Users.UserQueries.GetUserRoles;
using GraphQL.Permissions;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQueries
    {
        [GraphQLDescription("Represents The Users of The Api")]
        [UsePaging]
        //[UseProjection]
        [UseFiltering]
        [UseSorting]
        public async Task<List<UserDetailsDto>> GetUsers(IMediator mediator,
        CancellationToken ct, [Service] ILogger<UserQueries> _log)
        {
            _log.LogInformation("Testing Logging");
            return await mediator.Send(new GetAllUsersQuery(), ct);
        }

    }

    [ExtendObjectType(typeof(UserDetailsDto))]
    public class UserExtension
    {
        //[Authorize]
        public async Task<List<UserRoleDto>> GetRoles(
            [Parent] UserDetailsDto user, IMediator mediator)
        {
            var id = user.Id!;
            return await mediator.Send(new GetUserRolesQuery() { UserId = id }, new CancellationToken());
        }

        [GQLMustHavePermission(ApiAction.View, ApiResource.Users)]
        public async Task<List<UserRoleDto>> GetRoles2(
            [Parent] UserDetailsDto user, IMediator mediator)
        {
            /*var id = user.Id!;
            return await mediator.Send(new GetUserRolesQuery() { UserId = id }, new CancellationToken());*/
            return new List<UserRoleDto>();
        }
        [UseFiltering]
        public async Task<List<string>> GetPermissions(
            [Parent] UserDetailsDto user, IMediator mediator)
        {
            var id = user.Id!;
            return await mediator.Send(new GetUserPermissionsQuery() { UserId = id }, new CancellationToken());
        }
    }
}