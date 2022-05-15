using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.Password.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.ConfirmNewPassword)
                .NotEmpty()
                .Equal(p => p.Password);

            RuleFor(p => p.Token)
                .NotEmpty();
        }
    }
}
