using Application;
using DealersAndDistributors.Server;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddApplication()
       .AddPresentation();

builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            //builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            policy
                .SetIsOriginAllowed(origin =>
                {
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
                    return uri.IsLoopback; // allow any localhost / loopback with any port and scheme
                })
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services       
       .AddInfrastructure(builder.Configuration, builder.Environment);


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseMiddleware();
app.UseInfrastructure(builder.Configuration);
app.MapEndpoints();

await app.RunAsync();
