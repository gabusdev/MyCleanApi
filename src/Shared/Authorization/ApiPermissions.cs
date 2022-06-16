using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class ApiAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class ApiResource
{
    public const string Notifications = nameof(Notifications);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Permissions = nameof(Permissions);
    public const string Tests = nameof(Tests);
}

public static class ApiPermissions
{
    private static readonly ApiPermission[] _all = new ApiPermission[]
    {
        new("View Users", ApiAction.View, ApiResource.Users),
        new("Search Users", ApiAction.Search, ApiResource.Users),
        new("Create Users", ApiAction.Create, ApiResource.Users),
        new("Update Users", ApiAction.Update, ApiResource.Users),
        new("Delete Users", ApiAction.Delete, ApiResource.Users, IsRoot: true),
        new("Export Users", ApiAction.Export, ApiResource.Users),

        new("View Notifications", ApiAction.View, ApiResource.Notifications, IsBasic: true),
        new("Search Notifications", ApiAction.Search, ApiResource.Notifications),
        new("Create Notifications", ApiAction.Create, ApiResource.Notifications),
        new("Update Notifications", ApiAction.Update, ApiResource.Notifications),
        new("Delete Notifications", ApiAction.Delete, ApiResource.Notifications, IsRoot: true),
        new("Export Notifications", ApiAction.Export, ApiResource.Notifications),

        new("View UserRoles", ApiAction.View, ApiResource.UserRoles),
        new("Update UserRoles", ApiAction.Update, ApiResource.UserRoles, IsRoot: true),

        new("View Roles", ApiAction.View, ApiResource.Roles),
        new("Search Roles", ApiAction.Search, ApiResource.Roles),
        new("Create Roles", ApiAction.Create, ApiResource.Roles, IsRoot: true),
        new("Update Roles", ApiAction.Update, ApiResource.Roles, IsRoot: true),
        new("Delete Roles", ApiAction.Delete, ApiResource.Roles, IsRoot: true),

        new("View RoleClaims", ApiAction.View, ApiResource.RoleClaims),
        new("Update RoleClaims", ApiAction.Update, ApiResource.RoleClaims, IsRoot: true),

        new("View Permissions", ApiAction.View, ApiResource.Permissions)

    };

    public static IReadOnlyList<ApiPermission> All { get; } = new ReadOnlyCollection<ApiPermission>(_all);
    public static IReadOnlyList<ApiPermission> NoRoot { get; } = new ReadOnlyCollection<ApiPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<ApiPermission> Basic { get; } = new ReadOnlyCollection<ApiPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record ApiPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}