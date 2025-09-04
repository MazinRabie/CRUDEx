using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ResultFilters
{
    public class AddTokenAuthorizationToCookiesFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
            await next();
        }
    }
}