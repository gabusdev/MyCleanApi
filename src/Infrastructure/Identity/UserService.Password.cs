using Application.Identity.Users.Password;

namespace Infrastructure.Identity
{
    internal partial class UserService
    {
        public Task ChangePasswordAsync(ChangePasswordRequest request, string userId)
        {
            throw new NotImplementedException();
        }
        public Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            throw new NotImplementedException();
        }
        public Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
