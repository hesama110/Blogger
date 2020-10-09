﻿using Microsoft.AspNetCore.Builder;

namespace ResponseMessageWrapper.Core.Extensions
{
    public static class ApiResponseMiddlewareExtension
    {
        public static IApplicationBuilder UseAPIResponseWrapperMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<APIResponseMiddleware>();
        }
    }
}
