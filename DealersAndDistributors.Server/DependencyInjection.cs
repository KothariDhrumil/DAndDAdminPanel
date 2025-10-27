using DealersAndDistributors.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Microsoft.OpenApi.Models;

namespace DealersAndDistributors.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DealersAndDistributors API", Version = "v1" });
        //    var jwtScheme = new OpenApiSecurityScheme
        //    {
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.Http,
        //        Scheme = "bearer",
        //        BearerFormat = "JWT",
        //        In = ParameterLocation.Header,
        //        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'"
        //    };
        //    c.AddSecurityDefinition("Bearer", jwtScheme);
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        { jwtScheme, Array.Empty<string>() }
        //    });
        //});
        services.AddSwaggerGen();

        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = c =>
            {
                var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                  .SelectMany(v => v.Errors)
                  .Select(v => v.ErrorMessage));

                return new BadRequestObjectResult(Result.Failure(Error.Problem("400",errors)));
            };
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Register text doc generator
       // services.AddHostedService<DealersAndDistributors.Server.Documentation.ApiTextDocHostedService>();

        return services;
    }
}
