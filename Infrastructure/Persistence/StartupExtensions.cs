using Application.Abstractions.Data;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class StartupExtensions
{
    private static string ProductDbContextHistoryName = "ProductDbContextHistory";

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddIdentity(configuration)
            .AddShardingDatabase(configuration)
            .AddRepositoryPattern();


    private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {

        string? conn = configuration.GetConnectionString("IdentityConnection");
        return services
            .AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));


    }
    private static IServiceCollection AddShardingDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RetailDbContext>(options =>
             options.UseSqlServer(
                 configuration.GetConnectionString("DefaultConnection"), dbOptions =>
             dbOptions.MigrationsHistoryTable(ProductDbContextHistoryName)));

        services.AddScoped<IRetailDbContext>(sp => sp.GetRequiredService<RetailDbContext>());

        return services;
    }

    private static IServiceCollection AddRepositoryPattern(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repositories.Repository<>));

        return services;
    }

}

