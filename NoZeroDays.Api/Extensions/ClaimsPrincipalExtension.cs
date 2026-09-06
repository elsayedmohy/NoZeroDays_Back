using System.Security.Claims;

namespace NoZeroDays.Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string? GetIdentityId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? identityId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return identityId;
    }
}
