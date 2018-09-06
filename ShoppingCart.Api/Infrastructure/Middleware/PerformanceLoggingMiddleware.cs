using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.Middleware
{
    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _log;

        public PerformanceLoggingMiddleware(RequestDelegate next, ILogger log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await _next.Invoke(context);

            stopWatch.Stop();

            _log.Information(
              "Request: {@Method} {@Path} executed in {RequestTime:000} ms",
              context.Request.Method, context.Request.Path, stopWatch.ElapsedMilliseconds);            
        }
    }

    public static class PerformanceLoggingExtension
    {
        public static IApplicationBuilder UsePerformanceLogging(
          this IApplicationBuilder app,
          ILogger log)
        {
            return app.UseMiddleware<PerformanceLoggingMiddleware>(log);
        }
    }
}
