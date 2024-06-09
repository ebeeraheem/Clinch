using System.Security.Claims;

namespace ClinchApi.Extensions;

public static class UserHelper
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
