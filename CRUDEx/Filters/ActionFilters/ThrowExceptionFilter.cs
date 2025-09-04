using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ActionFilters
{
    public class ThrowExceptionFilter : IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new Exception("this is an exception manually raised for testing purposes");
        }
    }
}