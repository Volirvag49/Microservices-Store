using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Api.Infrastructure.CorrelationToken.Middleware
{
    public class CorrelationTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Guid correlationToken;

            if (!(context.Request.Headers["Correlation-Token"].ToString() != null 
                && Guid.TryParse(context.Request.Headers["Correlation-Token"], out correlationToken)))
            {
                correlationToken = Guid.NewGuid();
            }
                
            context.Request.Headers.Add("correlationToken", correlationToken.ToString());

            using (LogContext.PushProperty("CorrelationToken", correlationToken))
            {
                await _next.Invoke(context);
            }           
        }    
    }

    public static class CorrelationTokenMiddlewareExtension
    {
        public static IApplicationBuilder UseCorrelationToken(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationTokenMiddleware>();
        }
    }
}
