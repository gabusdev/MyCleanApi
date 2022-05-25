using Application.Identity.Roles;
using Application.Identity.Roles.Queries.GetAllRolesQuery;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetUserRoles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Queries
{
    public class Query2
    {
        public async Task<List<UserDetailsDto>> GetUsers([Service] IMediator mediator,
        CancellationToken ct)
        {
            return await mediator.Send(new GetAllUsersQuery(), ct);
        }

        public async Task<List<UserRoleDto>> GetRoleById([Service] IMediator mediator,
            string id,
            CancellationToken ct)
        {
            return await mediator.Send(new GetUserRolesQuery() { UserId = id}, ct);
        }
        public async Task<List<RoleDto>> GetRole([Service] IMediator mediator,
            CancellationToken ct)
        {
            return await mediator.Send(new GetAllRolesQuery(), ct);
        }
    }
    /*public class UserType : ObjectType<UserDetailsDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserDetailsDto> descriptor)
        {
            descriptor
                .Field("Roles")
                .Type<ListType<UserRoleDtoType>>()
                .ResolveWith<IMediator>(async r => {
                    var x = await r.Send(new GetUserRolesQuery() { UserId = "2" });
                    return x;
                });
            descriptor.Field("Permissions")
                .Type<ListType<StringType>>();
        }
    }
    public class UserRoleDtoType : ObjectType<UserRoleDto> { }
    */
 
}
