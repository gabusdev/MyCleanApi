using Application.Identity.Roles;
using Application.Identity.Roles.Queries.GetAllRolesQuery;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using Application.Identity.Users.UserQueries.GetUserRoles;
using HotChocolate.AspNetCore.Authorization;
using Infrastructure.Auth.Permissions;
using MediatR;
using Shared.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Queries
{
    public class QueryType : ObjectType<Query2>
    {
        protected override void Configure(IObjectTypeDescriptor<Query2> descriptor)
        {
            descriptor
                .Field("greeting")
                .Resolve((x, y) => "Hi!").Authorize();
        }
    }
    public class Query2
    {
        
        public async Task<List<UserDetailsDto>> GetUsers(IMediator mediator,
        CancellationToken ct)
        {
            return await mediator.Send(new GetAllUsersQuery(), ct);
        }
        
        [Authorize]
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
                .ResolveWith<Query2>((r) => r.GetRoleById(default, "", new CancellationToken()));
            descriptor.Field("Permissions")
                .Type<ListType<StringType>>();
        }
    }
    public class UserRoleDtoType : ObjectType<UserRoleDto> { }
    */
    public class QueryTypeExtrnsion : ObjectType<Query2>
    {
        protected override void Configure(IObjectTypeDescriptor<Query2> descriptor)
        {
            descriptor.Field("otro_campo")
                .Type<StringType>()
                .Resolve((context, ct) =>
                {
                    var p = "Mmmmmm... Delicioso";
                    return p;
                });
        }
    }
    [ExtendObjectType(typeof(Query2))]
    public class ProductExtensions
    {
        public string GetOtro(
            [Parent] Query2 product)
            {
                return "He he he he...";
            }
    }
    [ExtendObjectType(typeof(UserDetailsDto))]
    public class UserExtension
    {
        [Authorize]
        public async Task<List<UserRoleDto>> GetRoles(
            [Parent] UserDetailsDto user, IMediator mediator)
        {
            var id = user.Id!;
            return await mediator.Send(new GetUserRolesQuery() { UserId = id }, new CancellationToken());
        }
        /*protected override void Configure(IObjectTypeDescriptor<UserDetailsDto> descriptor)
        {
            descriptor
                .Field("roles1")
                .Type<ListType<StringType>>()
                .Resolve(context =>
                    new List<string>() { "a" }
                );
        }*/
    }
}
