using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ActionFilters
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> _logger;
        public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("this is from action filter executed (after)");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("this is from action filter executing (before )");
            var msg = Convert.ToString(context.HttpContext.Items["msg"]);
            _logger.LogInformation("Code red : {msg}", msg);

        }
    }
}