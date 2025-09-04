using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.ResourceFilters
{
    public class FeatureDisableResourceFilter : IAsyncResourceFilter
    {
        private readonly bool _disabled;

        public FeatureDisableResourceFilter(bool disabled)
        {
            _disabled = disabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (_disabled)
                // context.Result = new NotFoundResult();
                context.Result = new StatusCodeResult(501);
            else
            {
                await next();
            }
        }
    }
}