using Application.Identity.Users.Password;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal partial class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
        {
            return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name) is not null;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new NotFoundException("User Not Found.");

            return user.Adapt<UserDetailsDto>();
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.AsNoTracking().CountAsync(cancellationToken);
        }

        public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken);
            return users.Adapt<List<UserDetailsDto>>();
        }

        public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId)
                                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException("User Not Found.");

            /*
            bool isAdmin = await _userManager.IsInRoleAsync(user, FSHRoles.Admin);
            if (isAdmin)
            {
                throw new ConflictException(_localizer["Administrators Profile's Status cannot be toggled"]);
            }
            */

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);
        }
    }
}
