using Application.Common.CQRS;

namespace Application.Identity.Roles.Queries.GetRoleWithPermissionsQuery
{
    public class GetRoleWithPermissionsQuery : IQuery<RoleDto>
    {
        public string RoleId { get; set; } = null!;
        public class GetRoleWithPermissionsQueryHandler : IQueryHandler<GetRoleWithPermissionsQuery, RoleDto>
        {
            private readonly IRoleService _roleService;
            public GetRoleWithPermissionsQueryHandler(IRoleService roleService)
            {
                _roleService = roleService;
            }
            public async Task<RoleDto> Handle(GetRoleWithPermissionsQuery request, CancellationToken cancellationToken)
            {
                return await _roleService.GetByIdWithPermissionsAsync(request.RoleId, cancellationToken);
            }
        }
    }
}
