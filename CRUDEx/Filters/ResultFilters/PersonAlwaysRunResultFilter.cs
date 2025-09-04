using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        private readonly ILogger<PersonAlwaysRunResultFilter> _logger;

        public PersonAlwaysRunResultFilter(ILogger<PersonAlwaysRunResultFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.Any(x => x.GetType() == typeof(SkipFilter.SkipFilter)))
            {
                _logger.LogInformation("{skipFilter} filter was skipped ", true);
                return;
            }
            _logger.LogInformation("{skipFilter} filter wasn't  skipped ", false);
        }
    }
}