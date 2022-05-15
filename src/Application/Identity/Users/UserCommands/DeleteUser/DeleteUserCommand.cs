using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserCommands.DeleteUser;

public class DeleteUserCommand: ICommand
{
    public string UserId { get; set; } = null!;

    public class DeleteUserCommandHandler : IdentityCommandHandler<DeleteUserCommand>
    {
        public DeleteUserCommandHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
        {
        }

        public override async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(request.UserId, cancellationToken);
            return Unit.Value;
        }
    }

    public class DeleteUserCommandValidator: AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId).
                NotEmpty();
        }
    }
}
