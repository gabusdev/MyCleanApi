using Shared.Authorization;

namespace Application.Identity.Users;

public class SetUserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new();

    public class SetUserRolesCommand : SetUserRolesRequest, ICommand
    {
        public string UserId { get; set; } = null!;
    }
    public class SetUserRolesCommandHandler : IdentityCommandHandler<SetUserRolesCommand>
    {
        private readonly ICurrentUser _currentUser;
        public SetUserRolesCommandHandler(IUserService userService, IHttpContextService httpContextService, ICurrentUser currentUser) : base(userService, httpContextService)
        {
            _currentUser = currentUser;
        }

        public async override Task<Unit> Handle(SetUserRolesCommand request, CancellationToken cancellationToken)
        {
            var current = _currentUser.GetUserId();
            var currentIsSuper = await _userService.HasRoleAsync(current, ApiRoles.SuperUser, cancellationToken);
            var currentIsAdmin = await _userService.HasRoleAsync(current, ApiRoles.Admin, cancellationToken);

            var objective = request.UserId;
            var objectiveIsSuper = await _userService.HasRoleAsync(objective, ApiRoles.SuperUser, cancellationToken);
            var objectiveIsAdmin = await _userService.HasRoleAsync(objective, ApiRoles.Admin, cancellationToken);

            if (!currentIsSuper)
            {
                if(request.UserRoles.Find(ur => ur.RoleName == ApiRoles.SuperUser) is not null)
                {
                    throw new ForbiddenException("Only SuperUser can asign SuperUser Role");
                }

            }

            if (!currentIsSuper || !currentIsAdmin)
            {
                if (request.UserRoles.Find(ur => ur.RoleName == ApiRoles.Admin) is not null)
                {
                    throw new ForbiddenException("Only SuperUser can asign SuperUser Role");
                }

            }

            await _userService.AssignRolesAsync(request.UserId, request, cancellationToken);
            return Unit.Value;
        }
    }
}