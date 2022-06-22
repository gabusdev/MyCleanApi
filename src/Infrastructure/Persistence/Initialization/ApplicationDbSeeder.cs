using Infrastructure.Identity.Role;
using Infrastructure.Identity.User;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Authorization;

namespace Infrastructure.Persistence.Initialization
{
    internal class ApplicationDbSeeder : IApplicationDbSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationDbSeeder(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            Log.Information("Seeding Database");
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await SeedGuestUserAsync();
        }

        private async Task SeedAdminUserAsync()
        {
            var adminUserName = "admin".ToLowerInvariant();
            var adminUser = new ApplicationUser
            {
                FirstName = "Administrator".ToLowerInvariant(),
                LastName = ApiRoles.Admin,
                Email = "admin@mail.com",
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "admin@mail.com".ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
                IsActive = true
            };
            await SeedUser(adminUser, "admin", ApiRoles.Admin);
        }

        private async Task SeedGuestUserAsync()
        {
            string guestUsername = "guest".ToLowerInvariant();
            var guestUser = new ApplicationUser
            {
                FirstName = "Guest".ToLowerInvariant(),
                LastName = ApiRoles.Basic,
                Email = "guest@mail.com",
                UserName = guestUsername,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "guest@mail.com".ToUpperInvariant(),
                NormalizedUserName = guestUsername.ToUpperInvariant(),
                IsActive = true
            };
            await SeedUser(guestUser, "guest", ApiRoles.Basic);
        }

        private async Task SeedUser(ApplicationUser user, string pass, string role)
        {
            // Create User
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email)
                is not ApplicationUser existingUser)
            {
                Log.Information($"Seeding Default User '{user.FirstName} {user.LastName}'");
                var password = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = password.HashPassword(user, pass);
                await _userManager.CreateAsync(user);
            }
            else
            {
                user = existingUser;
            }
            // Assign role to user
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                Log.Information($"Assigning '{role}' Role to User '{user.FirstName} {user.LastName}'");
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private async Task SeedRolesAsync(ApplicationDbContext dbContext)
        {
            // Create Admin Role
            var adminRole = new ApplicationRole(ApiRoles.Admin, "Administrator Role", 0);
            await SeedRoleAsync(dbContext, adminRole, ApiPermissions.All);

            // Create Basic Role
            var basicRole = new ApplicationRole(ApiRoles.Basic, "Basic Role", 100);
            await SeedRoleAsync(dbContext, basicRole, ApiPermissions.Basic);
        }
        private async Task SeedRoleAsync(ApplicationDbContext dbContext, ApplicationRole role, IReadOnlyList<ApiPermission> permissions)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == role.Name)
                    is not ApplicationRole existingRole)
            {
                Log.Information($"Seeding {ApiRoles.Admin} Role");
                role = new ApplicationRole(role.Name, role.Description, role.SecurityLevel);
                await _roleManager.CreateAsync(role);
            }
            else
            {
                role = existingRole;
            }

            await AssignPermissionsToRoleAsync(dbContext, permissions, role);
        }
        private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<ApiPermission> permissions, ApplicationRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == ApiClaims.Permission && c.Value == permission.Name))
                {
                    Log.Information($"Seeding {role.Name} Permission '{permission.Name}'");
                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = ApiClaims.Permission,
                        ClaimValue = permission.Name,
                        CreatedBy = "ApplicationDbSeeder"
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
