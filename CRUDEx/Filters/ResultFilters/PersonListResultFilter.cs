using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ResultFilters
{
    public class PersonListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonListResultFilter> _logger;

        public PersonListResultFilter(ILogger<PersonListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            _logger.LogInformation("code blue {FilterName} -before", nameof(PersonListResultFilter));
            context.HttpContext.Response.Headers["last-Modeified"] = DateTime.Now.ToString("yyyy-MM-dd : HH:mm");
            await next();
            _logger.LogInformation("code blue {FilterName} -after", nameof(PersonListResultFilter));
        }
    }
}