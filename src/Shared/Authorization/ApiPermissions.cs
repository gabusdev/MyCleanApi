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
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
}

public static class ApiPermissions
{
    private static readonly ApiPermission[] _all = new ApiPermission[]
    {
        new("View Dashboard", ApiAction.View, ApiResource.Dashboard),
        new("View Hangfire", ApiAction.View, ApiResource.Hangfire),
        new("View Users", ApiAction.View, ApiResource.Users),
        new("Search Users", ApiAction.Search, ApiResource.Users),
        new("Create Users", ApiAction.Create, ApiResource.Users),
        new("Update Users", ApiAction.Update, ApiResource.Users),
        new("Delete Users", ApiAction.Delete, ApiResource.Users),
        new("Export Users", ApiAction.Export, ApiResource.Users),
        new("View UserRoles", ApiAction.View, ApiResource.UserRoles),
        new("Update UserRoles", ApiAction.Update, ApiResource.UserRoles),
        new("View Roles", ApiAction.View, ApiResource.Roles),
        new("Create Roles", ApiAction.Create, ApiResource.Roles),
        new("Update Roles", ApiAction.Update, ApiResource.Roles),
        new("Delete Roles", ApiAction.Delete, ApiResource.Roles),
        new("View RoleClaims", ApiAction.View, ApiResource.RoleClaims),
        new("Update RoleClaims", ApiAction.Update, ApiResource.RoleClaims),
        new("View Products", ApiAction.View, ApiResource.Products, IsBasic: true),
        new("Search Products", ApiAction.Search, ApiResource.Products, IsBasic: true),
        new("Create Products", ApiAction.Create, ApiResource.Products),
        new("Update Products", ApiAction.Update, ApiResource.Products),
        new("Delete Products", ApiAction.Delete, ApiResource.Products),
        new("Export Products", ApiAction.Export, ApiResource.Products),
        new("View Brands", ApiAction.View, ApiResource.Brands, IsBasic: true),
        new("Search Brands", ApiAction.Search, ApiResource.Brands, IsBasic: true),
        new("Create Brands", ApiAction.Create, ApiResource.Brands),
        new("Update Brands", ApiAction.Update, ApiResource.Brands),
        new("Delete Brands", ApiAction.Delete, ApiResource.Brands),
        new("Generate Brands", ApiAction.Generate, ApiResource.Brands),
        new("Clean Brands", ApiAction.Clean, ApiResource.Brands),
        new("View Tenants", ApiAction.View, ApiResource.Tenants, IsRoot: true),
        new("Create Tenants", ApiAction.Create, ApiResource.Tenants, IsRoot: true),
        new("Update Tenants", ApiAction.Update, ApiResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", ApiAction.UpgradeSubscription, ApiResource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<ApiPermission> All { get; } = new ReadOnlyCollection<ApiPermission>(_all);
    public static IReadOnlyList<ApiPermission> Root { get; } = new ReadOnlyCollection<ApiPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<ApiPermission> Admin { get; } = new ReadOnlyCollection<ApiPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<ApiPermission> Basic { get; } = new ReadOnlyCollection<ApiPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record ApiPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}