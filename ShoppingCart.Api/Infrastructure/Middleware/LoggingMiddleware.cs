using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace ShoppingCart.Api.Infrastructure.Middleware
{
    public class RequestLogging
    {
        public static RequestDelegate Middleware(RequestDelegate next, ILogger log)
        {
            return async env =>
            {       
                log.Information(
                  "Incoming request: {@Method}, {@Path}, {@Headers}",
                  env.Request.Method,
                  env.Request.Path,
                  env.Request.Headers);
                await next(env);
                log.Information(
                  "Outgoing response: {@StatucCode}, {@Headers}",
                   env.Response.StatusCode,
                   env.Response.Headers);
            };
        }
    }

    public class LoggingMiddleware
    {
        private RequestDelegate _next;
    }
}
