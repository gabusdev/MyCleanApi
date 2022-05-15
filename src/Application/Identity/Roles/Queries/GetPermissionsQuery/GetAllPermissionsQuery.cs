using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Roles.Queries.GetPermissionsQuery
{
    public class GetAllPermissionsQuery: IQuery<List<string>>
    {
        public class GetAllPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, List<string>>
        {
            private readonly IRoleService _roleService;
            public GetAllPermissionsQueryHandler(IRoleService roleService)
            {
                _roleService = roleService;
            }
            public Task<List<string>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_roleService.GetAllPermissions());
            }
        }
    }
}
