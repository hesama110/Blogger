using Microsoft.AspNetCore.Builder;



namespace ExtensionsAttributes.Middleware.LogResponse
{

    public static class LogRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogResponse(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogResponseMiddleware>();
        }
    }
}
