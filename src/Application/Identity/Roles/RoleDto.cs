namespace Application.Identity.Roles;

public class RoleDto : IDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? SecurityLevel { get; set; }
    public List<string>? Permissions { get; set; }
}