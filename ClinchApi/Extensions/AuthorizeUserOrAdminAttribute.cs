using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ClinchApi.Extensions;

public class AuthorizeUserOrAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var currentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var isAdmin = user.IsInRole("Admin");

        var routeData = context.RouteData.Values;

        var userId = routeData.ContainsKey("userId") ? 
            routeData["userId"].ToString() : 
            null;

        if (string.IsNullOrEmpty(userId) || (currentUserId != userId && !isAdmin))
        {
            context.Result = new ForbidResult();
        }
    }
}
