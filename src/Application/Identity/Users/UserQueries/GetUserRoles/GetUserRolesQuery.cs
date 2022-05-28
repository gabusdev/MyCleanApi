namespace Application.Identity.Users.UserQueries.GetUserRoles
{
    public class GetUserRolesQuery : IQuery<List<UserRoleDto>>
    {
        public string UserId { get; set; } = null!;
        public class GetRolesQueryHandler : IdentityQueryHandler<GetUserRolesQuery, List<UserRoleDto>>
        {

            private readonly ICurrentUserService _currentUser;
            public GetRolesQueryHandler(IUserService userService, IHttpContextService httpContextService, ICurrentUserService currentUser) : base(userService, httpContextService)
            {
                _currentUser = currentUser;
            }

            public async override Task<List<UserRoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
            {
                /*
                var current = _currentUser.GetUserId();

                var currentIsAdmin = await _userService.HasRoleAsync(current, ApiRoles.Admin, cancellationToken);
                */

                var roles = await _userService.GetRolesAsync(request.UserId, cancellationToken);

                /*
                if (!currentIsAdmin)
                    roles = roles.Where(r => r.RoleName != ApiRoles.Admin).ToList();
                */
                return roles;
            }
        }
        public class GetRolesQueryHandlerValidator : AbstractValidator<GetUserRolesQuery>
        {
            public GetRolesQueryHandlerValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();
            }
        }
    }
}
