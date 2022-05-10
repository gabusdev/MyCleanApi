using Application.Identity.Users.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
