using P72.Api.Trading.Middleware;

namespace P72.Api.Trading.Extensions
{
    public static class ExceptionExtensions
    {
        private const string CommonMessage = "An error occurred while processing your request.";

        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static string GetExceptionDetails(this Exception exception)
        {
            var details = CommonMessage;
            details += $" Message - {exception.Message}, StackTrace - {exception.StackTrace}";
            return details;
        }
    }
}
