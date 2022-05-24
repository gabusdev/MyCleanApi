using Microsoft.Extensions.Localization;

namespace Application.Identity.Roles.Commands.CreateUpdateCommand;

public class CreateOrUpdateRoleCommand : ICommand
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public double SecurityLevel { get; set; }

    public class CreateOrUpdateRoleCommandHandler : ICommandHandler<CreateOrUpdateRoleCommand, Unit>
    {
        private readonly IRoleService _roleService;
        public CreateOrUpdateRoleCommandHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<Unit> Handle(CreateOrUpdateRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleService.CreateOrUpdateAsync(request);
            return Unit.Value;
        }
    }
}

public class CreateOrUpdateRoleRequestValidator : AbstractValidator<CreateOrUpdateRoleCommand>
{
    public CreateOrUpdateRoleRequestValidator(IRoleService roleService, IStringLocalizer<CreateOrUpdateRoleRequestValidator> localizer)
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
                .WithMessage(localizer["validation.role.similar"]);
        RuleFor(r => r.SecurityLevel)
            .NotEmpty()
            .GreaterThan(0)
                .WithMessage(localizer["validation.role.seclevel"]);
    }


}