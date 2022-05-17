using Application.Common.Mailing;
using Application.Identity.Users.UserCommands.ToggleUserStatus;
using Application.Identity.Users.UserQueries;
using Infrastructure.Auth;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Shared.Authorization;

namespace Infrastructure.Identity
{
    internal partial class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IEmailTemplateService _templateService;
        private readonly IMailService _mailService;
        private readonly SecuritySettings _securitySettings;

        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext db,
            IStringLocalizer<UserService> localizer,
            IEmailTemplateService templateService,
            IMailService mailService,
            IOptions<SecuritySettings> securitySettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _localizer = localizer;
            _templateService = templateService;
            _mailService = mailService;
            _securitySettings = securitySettings.Value;
        }

        public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
        {
            return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithNameAsync(string name, string? exceptId = null)
        {
            return await _userManager.FindByNameAsync(name) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _ = user ?? throw new NotFoundException(_localizer["identity.usernotfound"]);

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

            _ = user ?? throw new NotFoundException(_localizer["identity.usernotfound"]);

            bool isAdmin = await _userManager.IsInRoleAsync(user, ApiRoles.Admin);
            if (isAdmin)
            {
                throw new ConflictException(_localizer["identity.notallowed"]);
            }

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);
        }
    }
}
