using HotChocolate.AspNetCore.Authorization;
using Shared.Authorization;

namespace GraphQL.Permissions;

public class GQLMustHavePermissionAttribute : AuthorizeAttribute
{
    public GQLMustHavePermissionAttribute(string action, string resource) =>
            Policy = ApiPermission.NameFor(action, resource);
}