using Infrastructure.Identity;
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
            await SeedSuperUserAsync();
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

        private async Task SeedSuperUserAsync()
        {
            string superUserName = "superUser".ToLowerInvariant();
            var superUser = new ApplicationUser
            {
                FirstName = "SuperUser".ToLowerInvariant(),
                LastName = ApiRoles.SuperUser,
                Email = "superuser@mail.com",
                UserName = superUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = "superuser@mail.com".ToUpperInvariant(),
                NormalizedUserName = superUserName.ToUpperInvariant(),
                IsActive = true
            };
            await SeedUser(superUser, "superuser", ApiRoles.SuperUser);
        }

        private async Task SeedUser(ApplicationUser user, string pass, string role)
        {
            // Create User
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email)
                is not ApplicationUser adminUser)
            {
                Log.Information($"Seeding Default User '{user.FirstName} {user.LastName}'");
                var password = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = password.HashPassword(user, pass);
                await _userManager.CreateAsync(user);
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
            foreach (string roleName in ApiRoles.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                    is not ApplicationRole role)
                {
                    // Create the role
                    Log.Information($"Seeding {roleName} Role");
                    role = new ApplicationRole(roleName, $"Default App {roleName} Role");
                    await _roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == ApiRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.Basic, role);
                }
                else if (roleName == ApiRoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.Admin, role);
                }
                else if (roleName == ApiRoles.SuperUser)
                {
                    await AssignPermissionsToRoleAsync(dbContext, ApiPermissions.All, role);
                }
            }
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
