using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _log;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            _log.Information(
                "Incoming request: {@Method}, {@Path}, {@Headers}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers);

            await _next.Invoke(context);
        }

    }

    public static class RequestLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, ILogger log)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>(log);
        }
    }
}
