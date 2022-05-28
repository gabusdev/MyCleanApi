using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetUserPermissions;
using Application.Identity.Users.UserQueries.GetUserRoles;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQueries
    {
        [GraphQLDescription("Represents The Users of The Api")]
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
        public async Task<List<string>> GetPermissions(
            [Parent] UserDetailsDto user, IMediator mediator)
        {
            var id = user.Id!;
            return await mediator.Send(new GetUserPermissionsQuery() { UserId = id }, new CancellationToken());
        }
    }
}
/*[Authorize]
        public async Task<List<UserRoleDto>> GetRoleById([Service] IMediator mediator,
            string id,
            CancellationToken ct)
        {
            return await mediator.Send(new GetUserRolesQuery() { UserId = id}, ct);
        }*/
/*public async Task<List<RoleDto>> GetRole([Service] IMediator mediator,
    CancellationToken ct)
{
    return await mediator.Send(new GetAllRolesQuery(), ct);
}*/