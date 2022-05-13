using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initialization
{
    internal class ApplicationDbSeeder : IApplicationDbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationDbSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SeedDatabaseAsync(DbContext dbContext, CancellationToken cancellationToken)
        {
            Log.Information("Seeding Database");
            // await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await SeedGuestUserAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == "admin@mail.com")
                is not ApplicationUser adminUser)
            {
                string adminUserName = "admin".ToLowerInvariant();
                adminUser = new ApplicationUser
                {
                    FirstName = "Administrator".ToLowerInvariant(),
                    //LastName = FSHRoles.Admin,
                    LastName = "Admin",
                    Email = "admin@mail.com",
                    UserName = adminUserName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = "admin@mail.com".ToUpperInvariant(),
                    NormalizedUserName = adminUserName.ToUpperInvariant(),
                    IsActive = true
                };

                Log.Information("Seeding Default Admin User");
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, "admin");
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            /*
            if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Admin))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                await _userManager.AddToRoleAsync(adminUser, FSHRoles.Admin);
            }
            */
        }

        private async Task SeedGuestUserAsync()
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == "guest@mail.com")
                is not ApplicationUser adminUser)
            {
                string guestUsername = "guest".ToLowerInvariant();
                adminUser = new ApplicationUser
                {
                    FirstName = "Guest".ToLowerInvariant(),
                    //LastName = FSHRoles.Admin,
                    LastName = "Basic",
                    Email = "guest@mail.com",
                    UserName = guestUsername,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = "guest@mail.com".ToUpperInvariant(),
                    NormalizedUserName = guestUsername.ToUpperInvariant(),
                    IsActive = true
                };

                Log.Information("Seeding Default Guest User");
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, "guest");
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            /*
            if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Admin))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                await _userManager.AddToRoleAsync(adminUser, FSHRoles.Admin);
            }
            */
        }
    }
}
