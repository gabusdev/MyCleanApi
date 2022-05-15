using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task ChangePasswordAsync(ChangePasswordCommand request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (!result.Succeeded)
                throw new ConflictException(result.Errors.First().Description);
        }
        public async Task<string> ForgotPasswordAsync(ForgotPasswordQuery request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }
        public async Task ResetPasswordAsync(ResetPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
                throw new ConflictException(result.Errors.First().Description);
        }
    }
}
