using AuthPermissions.AdminCode;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RunMethodsSequentially;

namespace Infrastructure.Auth.AuthP;

/// <summary>
/// If there are no RetailOutlets in the RetailDbContext it seeds the RetailDbContext with RetailOutlets and gives each of them some stock
/// </summary>

public class StartupServiceServiceSeedRetailDatabase : IStartupServiceToRunSequentially
{
    public int OrderNum { get; } //runs after the RetailDbContext has been migrated

    public async ValueTask ApplyYourChangeAsync(IServiceProvider scopedServices)
    {
        var context = scopedServices.GetRequiredService<RetailDbContext>();
        var numRetail = await context.RetailOutlets.IgnoreQueryFilters().CountAsync();
        if (numRetail == 0)
        {
            var authTenantAdmin = scopedServices.GetRequiredService<IAuthTenantAdminService>();

            var service = new SeedShopsOnStartup(context, authTenantAdmin);
            await service.CreateShopsAndSeedStockAsync(SeedShopsOnStartup.SeedStockText);
        }
    }
}