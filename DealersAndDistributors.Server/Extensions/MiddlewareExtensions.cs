
using DealersAndDistributors.Server.Middleware;

namespace DealersAndDistributors.Server;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        //app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
}
