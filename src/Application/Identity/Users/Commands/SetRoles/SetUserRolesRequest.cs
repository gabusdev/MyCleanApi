using Application.Identity.Users.Queries;
using Shared.Authorization;

namespace Application.Identity.Users.Commands.SetRoles;

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

        public override async Task<Unit> Handle(SetUserRolesCommand request, CancellationToken cancellationToken)
        {
            // Get the current User and if its admin
            var current = _currentUser.GetUserId();
            var currentIsAdmin = await _userService.HasRoleAsync(current, ApiRoles.Admin, cancellationToken);

            // Get the objective User and if its admin
            var objective = request.UserId;
            var objectiveIsAdmin = await _userService.HasRoleAsync(objective, ApiRoles.Admin, cancellationToken);

            // If the objective user is admin only can edit not Admin roles and by other Admin user
            if (objectiveIsAdmin)
            {
                if (!currentIsAdmin)
                {
                    throw new ForbiddenException("Not Enough Permissions");
                }
                else
                {
                    request.UserRoles = request.UserRoles.Where(ur => ur.RoleName != ApiRoles.Admin).ToList();
                }
            }
            // Else, if Security Level of current User is Lower from Objective Security Level, stop operation
            else
            {
                if (await _userService.GetSecurityLevel(current, cancellationToken)
                >= await _userService.GetSecurityLevel(objective, cancellationToken))
                {
                    throw new ForbiddenException("Can't Change Roles of Same or Higher Role Objective");
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