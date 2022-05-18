using Application.Common.Mailing;
using Application.Identity.Users.Password.Commands.ChangePassword;
using Application.Identity.Users.Password.Commands.ResetPassword;
using Application.Identity.Users.Password.Queries.ForgotPasswordQuery;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public async Task ChangePasswordAsync(ChangePasswordCommand request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new InternalServerException(_localizer["generic.error"]);

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (!result.Succeeded)
                throw new ConflictException(_localizer["generic.error"]);
        }
        public async Task<string> ForgotPasswordAsync(ForgotPasswordQuery request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            _ = user ?? throw new InternalServerException(_localizer["generic.error"]);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            const string route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", token);
            var mailRequest = new MailRequest(
                new List<string> { request.Email },
                _localizer["Reset Password"],
                _localizer[$"Your Password Reset Token is '{token}'. You can reset your password using the {passwordResetUrl} Endpoint."]);
            
            _jobService.Enqueue(() => _mailService.SendAsync(mailRequest));
            
            return "Password Reset Mail Sent";
        }
        public async Task ResetPasswordAsync(ResetPasswordCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            _ = user ?? throw new InternalServerException(_localizer["generic.error"]);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
                throw new ValidationException(_localizer["validation.errors"], result.GetErrors());
        }
    }
}
