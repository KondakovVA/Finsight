using Finsight.WebApi.Middleware;

namespace Finsight.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
