
using DealersAndDistributors.Server.Middleware;

namespace DealersAndDistributors.Server;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        //app.UseMiddleware<RequestContextLoggingMiddleware>();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        return app;
    }
}
