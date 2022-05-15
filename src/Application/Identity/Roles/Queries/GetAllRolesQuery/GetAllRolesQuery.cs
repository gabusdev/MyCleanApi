using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Roles.Queries.GetAllRolesQuery
{
    public class GetAllRolesQuery: IQuery<List<RoleDto>>
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
