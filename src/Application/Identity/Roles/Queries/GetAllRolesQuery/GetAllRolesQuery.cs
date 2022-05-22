using Application.Common.CQRS;

namespace Application.Identity.Roles.Queries.GetAllRolesQuery
{
    public class GetAllRolesQuery : IQuery<List<RoleDto>>
    {
        public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, List<RoleDto>>
        {
            private readonly IRoleService _roleService;
            public GetAllRolesQueryHandler(IRoleService roleService)
            {
                _roleService = roleService;
            }
            public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            {
                return await _roleService.GetListAsync(cancellationToken);
            }
        }
    }
}
