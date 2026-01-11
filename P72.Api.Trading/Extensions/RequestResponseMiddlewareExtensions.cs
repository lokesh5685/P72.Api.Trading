using P72.Api.Trading.Middleware;

namespace P72.Api.Trading.Extensions
{
    public static class RequestResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseMiddleware>();
        }
    }

}
