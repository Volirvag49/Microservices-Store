using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.Middleware
{
    public class GlobalErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _log;
        public GlobalErrorLoggingMiddleware(RequestDelegate next, ILogger log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Unhandled exception");
                throw;
            }
        }
    }

    public static class GlobalErrorLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalErrorLogging(this IApplicationBuilder app, ILogger log)
        {
            return app.UseMiddleware<GlobalErrorLoggingMiddleware>(log);
        }
    }
}
