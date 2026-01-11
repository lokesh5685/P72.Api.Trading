using P72.Api.Common;
using P72.Api.Trading.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Reflection.Metadata;
using Constant = P72.Api.Common.Constant;

namespace P72.Api.Trading.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string logMessage = $"An unhandled exception has occurred, {exception.Message}";
            logger.LogError(exception, logMessage);

            var statusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            TradApiErrorResponse result;
            if (exception.ToString().Contains("unexpected") || exception.ToString().Contains("Unexpected character encountered while parsing value"))
            {
                result = new TradApiErrorResponse(statusCode, Constant.RequestFormatMessage);
            }
            else
            {
                result = new TradApiErrorResponse(statusCode, Constant.InternalServerErrorMsg);
            }
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
