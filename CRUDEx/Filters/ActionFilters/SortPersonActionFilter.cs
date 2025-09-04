using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDEx.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ActionFilters
{
    public class SortPersonActionFilter : IActionFilter
    {
        private readonly ILogger<SortPersonActionFilter> _logger;

        public SortPersonActionFilter(ILogger<SortPersonActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("sort actionFilter executed ");
            var controller = (PersonController)context.Controller;
            controller.ViewBag.hello = "hello";
            var Paramters = (IDictionary<string, object?>?)context.HttpContext.Items["Paramters"];
            if (Paramters != null)
            {
                if (Paramters.ContainsKey("sortBy"))
                {
                    _logger.LogInformation("final value for {sortBy}", Paramters["sortBy"]);
                }
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("sort actionFilter executing ");
            var sortBy = Convert.ToString(context.ActionArguments["sortBy"]);
            _logger.LogInformation("the value of {sortBy}", sortBy);

            List<string> allowedSortByLst = new List<string>()
            {
                "Name","Email","age","Gender","Country"
            };
            context.HttpContext.Items["Paramters"] = context.ActionArguments;
            if (context.ActionArguments.ContainsKey("sortBy"))
            {
                if (allowedSortByLst.Any(x => x == sortBy) == false)
                {
                    context.ActionArguments["sortBy"] = "Name";
                }
            }
        }
    }
}