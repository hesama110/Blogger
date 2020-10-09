using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using ResponseMessageWrapper.Core.Wrappers;
using ResponseMessageWrapper.Core.Extensions;
using System.IO;

namespace ResponseMessageWrapper.Core
{
    public class APIResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public APIResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context) || IsHomePage(context))
                try
                {
                await this._next(context);

                }
                catch (Exception ex)
                {

                    
                }
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await _next.Invoke(context);
                        if (!IsFile(context))
                        {
                            var body = await FormatResponse(context.Response);
                            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);

                            }
                            else
                            {
                                await HandleNotSuccessRequestAsync(context, body, context.Response.StatusCode);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        await HandleExceptionAsync(context, ex);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            ApiError apiError = null;
            APIResponse apiResponse = null;
            int code = 0;

            if (exception is ApiException)
            {
                var ex = exception as ApiException;
                apiError = new ApiError(ex.Message);
                apiError.ValidationErrors = ex.Errors;
                apiError.ReferenceErrorCode = ex.ReferenceErrorCode;
                apiError.ReferenceDocumentLink = ex.ReferenceDocumentLink;
                code = ex.StatusCode;
                context.Response.StatusCode = code;

            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError("Unauthorized Access");
                code = (int)HttpStatusCode.Unauthorized;
                context.Response.StatusCode = code;
            }
            else
            {
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = exception.GetBaseException().Message;
                string stack = exception.StackTrace;
#endif

                apiError = new ApiError(msg);
                apiError.Details = stack;
                code = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = code;
            }

            context.Response.ContentType = "application/json";

            apiResponse = new APIResponse(code, ResponseMessageEnum.Exception.GetDescription(), null, apiError);

            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);
        }

        private static Task HandleNotSuccessRequestAsync(HttpContext context, object body, int code)
        {
            context.Response.ContentType = "application/json";

            ApiError apiError = null;

            string jsonString, bodyText = string.Empty;
            APIResponse apiResponse = null;


            if (!body.ToString().IsValidJson())
                bodyText = JsonConvert.SerializeObject(body);
            else
                bodyText = body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type;

            type = bodyContent?.GetType();
            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                apiResponse = JsonConvert.DeserializeObject<APIResponse>(bodyText);
            }
            if (apiResponse == null || apiResponse.Result == null)
            {
                if (code == (int)HttpStatusCode.NotFound)
                    apiError = new ApiError("The specified URI does not exist. Please verify and try again.");
                else if (code == (int)HttpStatusCode.NoContent)
                    apiError = new ApiError("The specified URI does not contain any content.");
                else
                    apiError = new ApiError($"Your request cannot be processed. Please contact a support. {((HttpStatusCode)code).ToString()}");

                apiResponse = new APIResponse(code, ResponseMessageEnum.Failure.GetDescription(), null, apiError);
            }
            context.Response.StatusCode = code;

            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);
        }

        private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            APIResponse apiResponse = null;


            if (!body.ToString().IsValidJson())
                bodyText = JsonConvert.SerializeObject(body);
            else
                bodyText = body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type;

            type = bodyContent?.GetType();

            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                apiResponse = JsonConvert.DeserializeObject<APIResponse>(bodyText);
                if (apiResponse.StatusCode != code && apiResponse.Result != null)
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                else if (apiResponse.Result != null)
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                else
                {
                    apiResponse = new APIResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);
                    jsonString = JsonConvert.SerializeObject(apiResponse);
                }
            }
            else
            {
                apiResponse = new APIResponse(code, ResponseMessageEnum.Success.GetDescription(), bodyContent, null);
                jsonString = JsonConvert.SerializeObject(apiResponse);
            }

            return context.Response.WriteAsync(jsonString);
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");

        }

        private bool IsHomePage(HttpContext context)
        {
            return context.Request.Path.Equals("/");

        }

        private bool IsFile(HttpContext context)
        {
            string mimetype = context.Response.ContentType?.ToLower();
            switch (mimetype)
            {
                case "image/png":
                case "image/gif":
                case "image/jpeg":
                case "image/bmp":
                case "image/tiff":
                case "image/wmf":
                case "image/jp2":
                case "image/svg+xml":
                case "application/octet-stream":
                case "application/javascript; charset=utf-8":
                    return true;
                default:
                    return false;
            }

        }
    }
}
