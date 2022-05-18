using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Role;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public double SecurityLevel { get; set; }

    public ApplicationRole(string name, string? description = null, double securityLevel = 100)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
        SecurityLevel = securityLevel;
    }
}