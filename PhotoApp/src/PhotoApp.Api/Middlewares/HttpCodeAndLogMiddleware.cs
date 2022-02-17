using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PhotoApp.Core.Exceptions;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace PhotoApp.Api.Middlewares
{
    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpCodeAndLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpCodeAndLogMiddleware>();
        }
    }

    public class HttpCodeAndLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpCodeAndLogMiddleware> _logger;

        public HttpCodeAndLogMiddleware(RequestDelegate next, ILogger<HttpCodeAndLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return;
            }

            try
            {
                httpContext.Request.EnableBuffering();
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case ApiException e:
                        // custom application error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await WriteAndLogResponseAsync(ex, httpContext,
                            HttpStatusCode.BadRequest, LogLevel.Error, "BadRequest Exception! " + ex.Message);
                        break;
                    case NotFoundException e:
                        // not found error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        await WriteAndLogResponseAsync(ex, httpContext,
                            HttpStatusCode.NotFound, LogLevel.Error, "NotFound Exception! " + ex.Message);
                        break;
                    case ValidationException e:
                        // validation error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        await WriteAndLogResponseAsync(ex, httpContext,
                            HttpStatusCode.UnprocessableEntity, LogLevel.Error, "Validation Exception! " + ex.Message);
                        break;
                    case AuthenticationException e:
                        // validation error
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await WriteAndLogResponseAsync(ex, httpContext,
                            HttpStatusCode.Unauthorized, LogLevel.Error, "Authentication Exception! " + ex.Message);
                        break;
                    default:
                        // unhandle error
                        await WriteAndLogResponseAsync(ex, httpContext,
                             HttpStatusCode.InternalServerError, LogLevel.Error, "Server error! " + ex.Message);
                        break;
                }
            }
        }

        private async Task WriteAndLogResponseAsync(Exception exception, HttpContext httpContext,
            HttpStatusCode httpStatusCode, LogLevel logLevel, string alternateMessage = null)
        {
            string requestBody = string.Empty;
            if (httpContext.Request.Body.CanSeek)
            {
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(httpContext.Request.Body))
                {
                    requestBody= JsonConvert.SerializeObject(sr.ReadToEndAsync());
                }
            }

            StringValues authorization;
            httpContext.Request.Headers.TryGetValue("Authorization", out authorization);
            StringBuilder customeDetails = new StringBuilder();

            #region configFormat
            customeDetails
                .AppendFormat("\n   Service URL    :").Append(httpContext.Request.Path.ToString())
                .AppendFormat("\n   Request Method :").Append(httpContext.Request?.Method)
                .AppendFormat("\n   Request Body   :").Append(requestBody)
                .AppendFormat("\n   Authorization  :").Append(authorization)
                .AppendFormat("\n   Content-Type   :").Append(httpContext.Request?.Headers["Content-Type"].ToString())
                .AppendFormat("\n   Cookie         :").Append(httpContext.Request?.Headers["Cookie"].ToString())
                .AppendFormat("\n   Host           :").Append(httpContext.Request?.Headers["Host"].ToString())
                .AppendFormat("\n   Referer        :").Append(httpContext.Request?.Headers["Referer"].ToString())
                .AppendFormat("\n   Origin         :").Append(httpContext.Request?.Headers["Origin"].ToString())
                .AppendFormat("\n   User-Agent     :").Append(httpContext.Request?.Headers["User-Agent"].ToString())
                .AppendFormat("\n   Error Message  :").Append(exception.Message);
            #endregion

            this._logger.Log(logLevel, exception, customeDetails.ToString());

            if (httpContext.Response.HasStarted)
            {
                this._logger.LogError("The response has already started, the http status code middleware will not be executed.");
                return;
            }

            string responseMessage = JsonConvert.SerializeObject(
                new
                {
                    Message = string.IsNullOrWhiteSpace(exception.Message) ? alternateMessage : exception.Message,
                });

            httpContext.Response.Clear();
            httpContext.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
            httpContext.Response.StatusCode = (int)httpStatusCode;
            await httpContext.Response.WriteAsync(responseMessage, Encoding.UTF8);
        }
    }
}
