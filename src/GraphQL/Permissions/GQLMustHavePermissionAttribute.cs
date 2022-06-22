using Shared.Authorization;
using AuthorizeAttribute = HotChocolate.AspNetCore.Authorization.AuthorizeAttribute;

namespace GraphQL.Permissions;

public class GQLMustHavePermissionAttribute : AuthorizeAttribute
{
    public GQLMustHavePermissionAttribute(string action, string resource) =>
            Policy = ApiPermission.NameFor(action, resource);
}