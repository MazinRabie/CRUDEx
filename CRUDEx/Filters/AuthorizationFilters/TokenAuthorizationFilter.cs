using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.AuthorizationFilters;

public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }
        if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }
    }
}