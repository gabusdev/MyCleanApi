using Application.Identity.Users.UserQueries;
using Shared.Authorization;

namespace Application.Identity.Users.UserCommands.SetRoles;

public class SetUserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new();

    public class SetUserRolesCommand : SetUserRolesRequest, ICommand
    {
        public string UserId { get; set; } = null!;
    }
    public class SetUserRolesCommandHandler : IdentityCommandHandler<SetUserRolesCommand>
    {
        private readonly ICurrentUserService _currentUser;
        public SetUserRolesCommandHandler(IUserService userService, IHttpContextService httpContextService, ICurrentUserService currentUser) : base(userService, httpContextService)
        {
            _currentUser = currentUser;
        }

        public async override Task<Unit> Handle(SetUserRolesCommand request, CancellationToken cancellationToken)
        {
            var current = _currentUser.GetUserId();
            var currentIsAdmin = await _userService.HasRoleAsync(current, ApiRoles.Admin, cancellationToken);

            var objective = request.UserId;
            var objectiveIsAdmin = await _userService.HasRoleAsync(objective, ApiRoles.Admin, cancellationToken);

            if (objectiveIsAdmin)
            {
                if (!currentIsAdmin)
                    throw new ForbiddenException("Not Enough Permissions");
                else
                    request.UserRoles = request.UserRoles.Where(ur => ur.RoleName != ApiRoles.Admin).ToList();
            }
            else
            {
                if (await _userService.GetSecurityLevel(current, cancellationToken)
                == await _userService.GetSecurityLevel(objective, cancellationToken))
                {
                    throw new ForbiddenException("Can't Change Roles of Same Role Objective");
                }
            }

            if (request.UserRoles.Find(ur => ur.RoleName == ApiRoles.Admin) is UserRoleDto adminRole)
            {
                if (currentIsAdmin)
                {
                    adminRole.Enabled = false;
                }
                else
                {
                    throw new ForbiddenException("Not Enough Permissions");
                }
            }

            await _userService.AssignRolesAsync(request.UserId, request, cancellationToken);
            return Unit.Value;
        }
    }
    public class SetUserRolesCommandValidator : AbstractValidator<SetUserRolesCommand>
    {
        public SetUserRolesCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty();
        }
    }
}