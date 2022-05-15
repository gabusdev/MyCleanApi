using Application.Identity.Users.UserQueries.GetById;
using Shared.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserQueries.GetRoles
{
    public class GetRolesQuery : IQuery<List<UserRoleDto>>
    {
        public string UserId { get; set; } = null!;
        public class GetRolesQueryHandler : IdentityQueryHandler<GetRolesQuery, List<UserRoleDto>>
        {

            private readonly ICurrentUser _currentUser;
            public GetRolesQueryHandler(IUserService userService, IHttpContextService httpContextService, ICurrentUser currentUser) : base(userService, httpContextService)
            {
                _currentUser = currentUser;
            }

            public async override Task<List<UserRoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var current = _currentUser.GetUserId();
                var roles = await _userService.GetRolesAsync(request.UserId, cancellationToken);

                if (await _userService.HasRoleAsync(current, ApiRoles.SuperUser, cancellationToken))
                    roles = roles.Where(r => r.RoleName != ApiRoles.SuperUser).ToList();
                
                return roles;
            }
        }
    }
}
