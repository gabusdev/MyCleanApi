using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types.Descriptors;
using Microsoft.AspNetCore.Authorization;
using Shared.Authorization;
using System.Reflection;
using AuthorizeAttribute = HotChocolate.AspNetCore.Authorization.AuthorizeAttribute;

namespace GraphQL.Permissions;

public class GQLMustHavePermissionAttribute : AuthorizeAttribute
{
    public GQLMustHavePermissionAttribute(string action, string resource) =>
            Policy = ApiPermission.NameFor(action, resource);
}