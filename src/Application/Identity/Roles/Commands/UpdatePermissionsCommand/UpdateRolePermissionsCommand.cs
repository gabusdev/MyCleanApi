using Application.Common.CQRS;

namespace Application.Identity.Roles.Commands.UpdatePermissionsCommand;

public class UpdateRolePermissionsCommand : ICommand
{
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;

    public class UpdateRolePermissionsCommandHandler : ICommandHandler<UpdateRolePermissionsCommand, Unit>
    {
        private readonly IRoleService _roleService;
        public UpdateRolePermissionsCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<Unit> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            await _roleService.UpdatePermissionsAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}

public class UpdateRolePermissionsRequestValidator : AbstractValidator<UpdateRolePermissionsCommand>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty();
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}