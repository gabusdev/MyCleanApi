using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserCommands.ToggleUserStatus
{
    public class ToggleUserStatusCommandValidator: AbstractValidator<ToggleUserStatusCommand>
    {
        public ToggleUserStatusCommandValidator()
        {
            RuleFor(c => c.ActivateUser)
                .NotEmpty();
            RuleFor(c => c.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}
