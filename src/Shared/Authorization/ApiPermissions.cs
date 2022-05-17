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
    //public const string Tenants = nameof(Tenants);
    //public const string Dashboard = nameof(Dashboard);
    //public const string Hangfire = nameof(Hangfire);

    //public const string Products = nameof(Products);
    //public const string Brands = nameof(Brands);

    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Permisions = nameof(Permisions);
    public const string Tests = nameof(Tests);
    public const string Self = nameof(Self);

}

public static class ApiPermissions
{
    private static readonly ApiPermission[] _all = new ApiPermission[]
    {
        //new("View Dashboard", ApiAction.View, ApiResource.Dashboard),
        //new("View Hangfire", ApiAction.View, ApiResource.Hangfire),
        /*
        new("View Products", ApiAction.View, ApiResource.Products, IsBasic: true),
        new("Search Products", ApiAction.Search, ApiResource.Products, IsBasic: true),
        new("Create Products", ApiAction.Create, ApiResource.Products),
        new("Update Products", ApiAction.Update, ApiResource.Products),
        new("Delete Products", ApiAction.Delete, ApiResource.Products),
        new("Export Products", ApiAction.Export, ApiResource.Products),
        */

        new("View Users", ApiAction.View, ApiResource.Users),
        new("Search Users", ApiAction.Search, ApiResource.Users),
        new("Create Users", ApiAction.Create, ApiResource.Users),
        new("Update Users", ApiAction.Update, ApiResource.Users),
        new("Delete Users", ApiAction.Delete, ApiResource.Users, IsRoot: true),
        new("Export Users", ApiAction.Export, ApiResource.Users),
        new("View UserRoles", ApiAction.View, ApiResource.UserRoles),
        new("Update UserRoles", ApiAction.Update, ApiResource.UserRoles, IsRoot: true),
        new("View Roles", ApiAction.View, ApiResource.Roles),
        new("Search Roles", ApiAction.Search, ApiResource.Roles),
        new("Create Roles", ApiAction.Create, ApiResource.Roles, IsRoot: true),
        new("Update Roles", ApiAction.Update, ApiResource.Roles, IsRoot: true),
        new("Delete Roles", ApiAction.Delete, ApiResource.Roles, IsRoot: true),
        new("View RoleClaims", ApiAction.View, ApiResource.RoleClaims),
        new("Update RoleClaims", ApiAction.Update, ApiResource.RoleClaims, IsRoot: true),
        new("View Permissions", ApiAction.View, ApiResource.Permisions),
        new("Use Tests Stuff", ApiAction.View, ApiResource.Tests)

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