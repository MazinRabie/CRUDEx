using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CRUDEx.Views.Shared
{
    public class ErrorView : PageModel
    {
        private readonly ILogger<ErrorView> _logger;

        public ErrorView(ILogger<ErrorView> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}