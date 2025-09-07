using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Middlewwares.Middleware
{
    public static class ExceptionHandlingMiddlewareMiddlewareExt
    {
        public static void UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }

    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        // "Scoped" SERVICE SHOULDN'T DO CONSTRUCTOR DI!!
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next(context);
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null)
                    _logger.LogError("{ExcptionFrom} : {exceptionType}  ------ msg {msg}", nameof(ExceptionHandlingMiddleware), ex.InnerException.GetType().ToString(), ex.InnerException.Message);
                else
                    _logger.LogError("{ExcptionFrom} : {exceptionType}  ------ msg {msg}", nameof(ExceptionHandlingMiddleware), ex.GetType(), ex.Message);
                // context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // context.Response.WriteAsync("Error occured Contact us for more help");

                throw;
            }
        }
    }
}


