using Application.Abstractions.Data;
using Application.Abstractions.Persistence;
using Application.Customers.Services;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Application.TenantUsers.Services;

public sealed class TenantUserOnboardingService : ITenantUserOnboardingService
{

    private readonly IRetailDbContext context;
    private readonly ITenantRetailDbContextFactory tenantRetailDbContextFactory;

    public TenantUserOnboardingService(IRetailDbContext context, ITenantRetailDbContextFactory tenantRetailDbContextFactory)
    {
        this.context = context;
        this.tenantRetailDbContextFactory = tenantRetailDbContextFactory;
    }

    // Method to create a tenant user profile by SuperAdmin if it doesn't exist
    public async Task CreateTenantUserBySuperAdmin(int tenantId, string globalUserId, string? displayName, string phoneNumber, CancellationToken ct)
    {
        var retailDbContext = await tenantRetailDbContextFactory.CreateAsync(tenantId, ct);

        await CreateTeantUser(globalUserId, displayName, phoneNumber, retailDbContext, ct);
    }

    private static async Task CreateTeantUser(string globalUserId, string? displayName, string phoneNumber, IRetailDbContext retailDbContext, CancellationToken ct)
    {
        var userId = Guid.Parse(globalUserId);
        var hasProfile = await retailDbContext.TenantUserProfiles
            .AnyAsync(p => p.GlobalUserId == userId, ct);
        if (!hasProfile)
        {
            retailDbContext.TenantUserProfiles.Add(new TenantUserProfile
            {
                GlobalUserId = userId,
                DisplayName = displayName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PhoneNumber = phoneNumber
            });
            await retailDbContext.SaveChangesAsync(ct);
        }
    }

    public async Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int TenantUserId, string? displayName, string phoneNumber, CancellationToken ct)
    {
        var userId = Guid.Parse(globalUserId);
        await CreateTeantUser(globalUserId, displayName, phoneNumber, context, ct);
    }

    // Method to update the  tenant user profile

    public async Task UpdateTenantUserProfileAsync(Guid globalUserId, string? displayName, CancellationToken ct)
    {
        var profile = await context.TenantUserProfiles
            .FirstOrDefaultAsync(p => p.GlobalUserId == globalUserId, ct);
        if (profile != null)
        {
            profile.DisplayName = displayName ?? profile.DisplayName;
            profile.UpdatedAt = DateTime.UtcNow;
            context.TenantUserProfiles.Update(profile);
            await context.SaveChangesAsync(ct);
        }
    }

}
