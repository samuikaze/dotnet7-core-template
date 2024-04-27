using DotNet7.Template.Api.Middlewares;

namespace DotNet7.Template.Api.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder ConfigureMiddlewares(this IApplicationBuilder builder)
        {
            // 引入 Middleware
            builder.UseMiddleware<ExampleMiddleware>();

            return builder;
        }
    }
}
