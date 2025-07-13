using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

public class CustomAuthorize : Attribute,IAuthorizationFilter
{
    private readonly string _requiredRoleId;

    public CustomAuthorize(string requiredRoleId)
    {
        _requiredRoleId = requiredRoleId;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var roleId = user.FindFirst("roleId")?.Value;

        if (roleId != _requiredRoleId)
        {
            context.Result = new ForbidResult();
        }
    }
}
