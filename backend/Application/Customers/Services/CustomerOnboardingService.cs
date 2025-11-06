using Application.Abstractions.Persistence;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Domain.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Services;

public sealed class CustomerOnboardingService : ICustomerOnboardingService
{
    private readonly AuthPermissionsDbContext authDb;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ITenantRetailDbContextFactory retailFactory;

    public CustomerOnboardingService(
        AuthPermissionsDbContext authDb,
        UserManager<ApplicationUser> userManager,
        ITenantRetailDbContextFactory retailFactory)
    {
        this.authDb = authDb;
        this.userManager = userManager;
        this.retailFactory = retailFactory;
    }

    public async Task<ApplicationUser> EnsureUserAsync(string phone, string firstName, string lastName, CancellationToken ct)
    {
        var email = $"{phone}@DandD.com";
        var pwd = $"{phone}@DandD";

        var existing = await userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phone, ct);
        if (existing != null)
            return existing;

        var user = new ApplicationUser
        {
            UserName = phone,
            PhoneNumber = phone,
            Email = email,
            PhoneNumberConfirmed = true,
            FirstName = firstName,
            LastName = lastName
        };
        var created = await userManager.CreateAsync(user, pwd);
        if (!created.Succeeded)
            throw new InvalidOperationException(string.Join(',', created.Errors.Select(e => e.Description)));

        return user;
    }

    public async Task<Guid> EnsureCustomerAccountAsync(ApplicationUser user, CancellationToken ct)
    {
        var existing = await authDb.CustomerAccounts.SingleOrDefaultAsync(c => c.GlobalUserId == user.Id, ct);
        if (existing != null)
            return existing.GlobalCustomerId;

        var account = new CustomerAccount
        {
            GlobalUserId = user.Id,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Status = "Active"
        };
        authDb.CustomerAccounts.Add(account);
        await authDb.SaveChangesAsync(ct);
        return account.GlobalCustomerId;
    }

    public async Task EnsureLinkedToTenantAsync(Guid globalCustomerId, int tenantId, string? firstName, string? lastName, string phoneNumber, CancellationToken ct)
    {
        var exists = await authDb.CustomerTenantLinks
            .AnyAsync(l => l.GlobalCustomerId == globalCustomerId && l.TenantId == tenantId, ct);
        if (!exists)
        {
            authDb.CustomerTenantLinks.Add(new CustomerTenantLink
            {
                GlobalCustomerId = globalCustomerId,
                TenantId = tenantId
            });
            await authDb.SaveChangesAsync(ct);
        }

        var retailDb = await retailFactory.CreateAsync(tenantId, ct);
        var hasProfile = await retailDb.TenantCustomerProfiles
            .AnyAsync(p => p.GlobalCustomerId == globalCustomerId, ct);
        if (!hasProfile)
        {
            retailDb.TenantCustomerProfiles.Add(new TenantCustomerProfile
            {
                GlobalCustomerId = globalCustomerId,
                TenantId = tenantId,
                FirstName = firstName,
                LastName = lastName,
                DataKey = retailDb.DataKey,
                HierarchyPath = $"|{globalCustomerId}|",
                Depth = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PhoneNumber = phoneNumber
            });
            await retailDb.SaveChangesAsync(ct);
        }
    }
}
