using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ActionFilters
{
    public class AddResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        private readonly ILogger<AddResponseHeaderActionFilter> _logger;
        private readonly string Key;
        private readonly string Value;
        public AddResponseHeaderActionFilter(ILogger<AddResponseHeaderActionFilter> logger, string key, string value, int order)
        {
            _logger = logger;
            Key = key;
            Value = value;
            Order = order;
        }
        public int Order { get; set; }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("executing {FilterName}.{MethodName}  - before ", nameof(AddResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            // just for trying the order  for exchange info
            context.HttpContext.Items["msg"] = "this is from add response header filter going to the personList filter";

            await next();
            _logger.LogInformation("executing {FilterName}.{MethodName} - after", nameof(AddResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}