using Shared.Authorization;

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

                var currentIsAdmin = await _userService.HasRoleAsync(current, ApiRoles.Admin, cancellationToken);
                var roles = await _userService.GetRolesAsync(request.UserId, cancellationToken);

                if (!currentIsAdmin)
                    roles = roles.Where(r => r.RoleName != ApiRoles.Admin).ToList();

                return roles;
            }
        }
    }
}
