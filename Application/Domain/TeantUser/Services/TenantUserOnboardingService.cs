using Application.Abstractions.Data;
using Application.Abstractions.Persistence;
using Application.Customers.Services;
using Application.Domain.TeantUser.Update;
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
    public async Task CreateTenantUserBySuperAdmin(int tenantId, string globalUserId, string firstName, string lastName, string phoneNumber, CancellationToken ct)
    {
        var retailDbContext = await tenantRetailDbContextFactory.CreateAsync(tenantId, ct);

        await CreateTeantUser(globalUserId, firstName, lastName, phoneNumber, retailDbContext, ct);
    }

    private static async Task CreateTeantUser(string globalUserId, string firstName, string lastName, string phoneNumber, IRetailDbContext retailDbContext, CancellationToken ct)
    {
        var userId = Guid.Parse(globalUserId);
        var hasProfile = await retailDbContext.TenantUserProfiles
            .AnyAsync(p => p.GlobalUserId == userId, ct);
        if (!hasProfile)
        {
            retailDbContext.TenantUserProfiles.Add(new TenantUserProfile
            {
                GlobalUserId = userId,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PhoneNumber = phoneNumber
            });
            await retailDbContext.SaveChangesAsync(ct);
        }
    }

    public async Task CreateTenantUserProfileIfMissingAsync(string globalUserId, int TenantUserId, string firstName, string lastName, string phoneNumber, CancellationToken ct)
    {
        var userId = Guid.Parse(globalUserId);
        await CreateTeantUser(globalUserId, firstName, lastName, phoneNumber, context, ct);
    }

    // Method to update the  tenant user profile

    public async Task UpdateTenantUserProfileAsync(UpdateTenantUserModel updateTenantUserModel, CancellationToken ct)
    {
        await UpdateProfile(context, updateTenantUserModel.UserId, updateTenantUserModel.FirstName, updateTenantUserModel.LastName, updateTenantUserModel.UserTypeId, ct);
    }

    private async Task UpdateProfile(IRetailDbContext retailDbContext, Guid globalUserId, string firstName, string lastName, int userTypeId, CancellationToken ct)
    {
        var profile = await retailDbContext.TenantUserProfiles
                    .FirstOrDefaultAsync(p => p.GlobalUserId == globalUserId, ct);
        if (profile != null)
        {
            profile.FirstName = firstName ?? profile.FirstName;
            profile.LastName = lastName ?? profile.LastName;
            profile.UpdatedAt = DateTime.UtcNow;
            profile.UserTypeId = userTypeId;
            retailDbContext.TenantUserProfiles.Update(profile);
            await retailDbContext.SaveChangesAsync(ct);
        }
    }

    //public async Task UpdateTenantUserBySuperadmin(int tenantId, string globalUserId, string firstName, string lastName, CancellationToken ct)
    //{
    //    var retailDbContext = await tenantRetailDbContextFactory.CreateAsync(tenantId, ct);
    //    var userId = Guid.Parse(globalUserId);
    //    await UpdateProfile(retailDbContext, userId, firstName, lastName, ct);

    //}
}
