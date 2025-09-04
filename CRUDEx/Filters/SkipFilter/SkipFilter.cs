using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEx.Filters.SkipFilter
{
    public class SkipFilter : Attribute, IFilterMetadata
    {

    }
}