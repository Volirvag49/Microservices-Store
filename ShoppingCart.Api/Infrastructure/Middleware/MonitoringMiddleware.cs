using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;
using Microsoft.AspNetCore.Owin;
using Microsoft.AspNetCore.Builder;

namespace ShoppingCart.Api.Infrastructure.Middleware
{
    public class MonitoringMiddleware
    {
        private RequestDelegate _next;
        private Func<Task<bool>> healthCheck;

        private static readonly PathString monitorPath = new PathString("/_monitor");
        private static readonly PathString monitorShallowPath = new PathString("/_monitor/shallow");
        private static readonly PathString monitorDeepPath = new PathString("/_monitor/deep");

        public MonitoringMiddleware(RequestDelegate next, Func<Task<bool>> healthCheck)
        {
            this._next = next;
            this.healthCheck = healthCheck;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(monitorPath))
                return HandleMonitorEndpoint(context);
            else
                return _next.Invoke(context);
        }

        private Task HandleMonitorEndpoint(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(monitorShallowPath))
                return ShallowEndpoint(context);
            else if (context.Request.Path.StartsWithSegments(monitorDeepPath))
                return DeepEndpoint(context);
            return Task.FromResult(0);
        }

        private async Task DeepEndpoint(HttpContext context)
        {
            if (await this.healthCheck())
                context.Response.StatusCode = 204;
            else
                context.Response.StatusCode = 503;
        }

        private Task ShallowEndpoint(HttpContext context)
        {
            context.Response.StatusCode = 204;
            return Task.FromResult(0);
        }

    }

    public static class MonitoringMiddlewareExtension
    {
        public static IApplicationBuilder UseMonitoring(
          this IApplicationBuilder app, Func<Task<bool>> healthCheck)
        {
           return app.UseMiddleware<MonitoringMiddleware>(healthCheck);           
        }
    }
}
