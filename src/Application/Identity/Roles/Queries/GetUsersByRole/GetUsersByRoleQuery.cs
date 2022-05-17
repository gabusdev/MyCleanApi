using Application.Identity.Users.UserQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Roles.Queries.GetUsersByRole
{
    public class GetUsersByRoleQuery: IQuery<List<UserDetailsDto>>
    {
        public string RoleId { get; set; } = null!;

        public class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, List<UserDetailsDto>>
        {
            private readonly IRoleService _roleService;
            public GetUsersByRoleQueryHandler(IRoleService roleService)
            {
                _roleService = roleService;
            }
            public async Task<List<UserDetailsDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
            {
                var users = await _roleService.GetUsersByIdAsync(request.RoleId);
                return users.ToList();
            }
        }


        public class GetUsersByRoleQueryValidator : AbstractValidator<GetUsersByRoleQuery>
        {
            public GetUsersByRoleQueryValidator()
            {
                RuleFor(x => x.RoleId)
                    .NotEmpty();
            }
        }
    }
}
