using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

internal static class IdentityResultExtensions
{
    /*
     *public static List<string> GetErrors(this IdentityResult result, IStringLocalizer localizer) =>
     *  result.Errors.Select(e => localizer[e.Description].ToString()).ToList();
    */
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(e => e.Description.ToString()).ToList();
}