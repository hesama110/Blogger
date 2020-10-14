using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Builder;




namespace ExtensionsAttributes.Middleware.LogRequest
{

    public static class LogRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogRequest(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogRequestMiddleware>();
        }
    }
}