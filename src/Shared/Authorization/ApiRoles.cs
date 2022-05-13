using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class ApiRoles
{
    public const string SuperUser = nameof(SuperUser);
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        SuperUser,
        Admin,
        Basic
    });

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}