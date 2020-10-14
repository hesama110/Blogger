using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionsAttributes.Middleware.DynamicallyRobotsTxt
{
    public static class RobotsTxtMiddlewareExtensions
    {
        public static IApplicationBuilder UseRobotsTxt(
            this IApplicationBuilder builder,
            string environmentName,
            string rootPath = null,
            string rootUrl = "http://localhost:5004"
        )
        {
            return builder.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/robots.txt"), b =>
                b.UseMiddleware<RobotsTxtMiddleware>(environmentName, rootPath, rootUrl));
        }
    }
}
