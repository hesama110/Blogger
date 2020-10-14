using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsAttributes.Middleware.DynamicallyRobotsTxt
{
    public class RobotsTxtMiddleware
    {
        //const string Default =
        //    @"User-Agent: *\nAllow: /\nSitemap: http://www.morningdew.info//sitemap.xml";
        StringBuilder Default = new StringBuilder().AppendLine("User-Agent: *").AppendLine("Allow: /");
        private readonly RequestDelegate next;
        private readonly string environmentName;
        private readonly string rootPath;

        public RobotsTxtMiddleware(
            RequestDelegate next,
            string environmentName,
            string rootPath,
            string rootUrl
        )
        {
            this.next = next;
            this.environmentName = environmentName;
            this.rootPath = rootPath;
            Default.AppendLine($"Sitemap: {rootUrl}/sitemap.xml");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/robots.txt",StringComparison.OrdinalIgnoreCase))
            {
                var generalRobotsTxt = Path.Combine(rootPath, "robots.txt");
                var environmentRobotsTxt = Path.Combine(rootPath, $"robots.{environmentName}.txt");
                string output;

                // try environment first
                if (File.Exists(environmentRobotsTxt))
                {
                    output = File.ReadAllText(environmentRobotsTxt);
                }
                // then robots.txt
                else if (File.Exists(generalRobotsTxt))
                {
                    output = File.ReadAllText(generalRobotsTxt);
                }
                // then just a general default
                else
                {
                    output = Default.ToString();
                }

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(output);
            }
            else
            {
                await next(context);
            }
        }
    }
}
