using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.User;

public class ApplicationUser : IdentityUser, IApplicationUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }

}