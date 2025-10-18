using Application.Abstractions.Authentication;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth.Jwt;

internal static class StartupExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(JwtSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!);

        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}