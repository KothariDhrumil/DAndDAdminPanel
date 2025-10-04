using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Application.Customers.Services;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Hierarchy;

public sealed class CreateChildCustomerCommand : ICommand<Guid>
{
    public Guid ParentGlobalCustomerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? TenantId { get; set; }
}

internal sealed class CreateChildCustomerCommandHandler(
    AuthPermissionsDbContext authDb,
    ITenantRetailDbContextFactory retailFactory,
    ICustomerOnboardingService provisioning)
    : ICommandHandler<CreateChildCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateChildCustomerCommand command, CancellationToken ct)
    {
        if (!command.TenantId.HasValue)
            return Result.Failure<Guid>(Error.Validation("TenantMissing", "Tenant id not resolved."));

        var tenantId = command.TenantId.Value;

        // Ensure parent profile exists in tenant
        var retailDb = await retailFactory.CreateAsync(tenantId, ct);
        var parentProfile = await retailDb.TenantCustomerProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.GlobalCustomerId == command.ParentGlobalCustomerId, ct);
        if (parentProfile == null)
            return Result.Failure<Guid>(Error.NotFound("ParentProfileNotFound", "Parent profile not found in tenant."));
        if (parentProfile.Depth >= 5)
            return Result.Failure<Guid>(Error.Validation("DepthLimit", "Maximum hierarchy depth reached."));

        var phone = command.PhoneNumber.Trim();
        

        // Ensure identity user + central customer
        var user = await provisioning.EnsureUserAsync(phone, command.FirstName, command.LastName, ct);
        var childGlobalId = await provisioning.EnsureCustomerAccountAsync(user, phone, user.Email, command.FirstName, command.LastName, ct);

        // Ensure central link + base profile
        var display = string.Join(' ', new[] { command.FirstName, command.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
        await provisioning.EnsureLinkedToTenantAsync(childGlobalId, tenantId, display, ct);

        // Now set hierarchy details if new profile (or update existing path)
        var childProfile = await retailDb.TenantCustomerProfiles
            .SingleAsync(p => p.GlobalCustomerId == childGlobalId, ct);

        childProfile.ParentGlobalCustomerId = parentProfile.GlobalCustomerId;
        childProfile.HierarchyPath = parentProfile.HierarchyPath + childGlobalId + "|";
        childProfile.Depth = (byte)(parentProfile.Depth + 1);
        childProfile.UpdatedAt = DateTime.UtcNow;

        await retailDb.SaveChangesAsync(ct);
        return Result.Success(childGlobalId);
    }
}
