using Infrastructure.Identity.User;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Identity
{
    partial class UserService
    {
        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            const string route = "api/v1/user/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == userId && !u.EmailConfirmed)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new InternalServerException(_localizer["generic.error"]);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded
                ? string.Format(_localizer["account.confirmed", user.Email])
                : throw new InternalServerException(string.Format(_localizer["generic.error"]));
        }
    }
}
