using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Domain;
using Domain.Customers;
using Microsoft.AspNetCore.Identity;
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
    UserManager<ApplicationUser> userManager,
    ITenantRetailDbContextFactory retailFactory)
    : ICommandHandler<CreateChildCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateChildCustomerCommand command, CancellationToken ct)
    {
        if (command.ParentGlobalCustomerId == Guid.Empty)
            return Result.Failure<Guid>(Error.Validation("ParentMissing", "Parent global customer id required."));
        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
            return Result.Failure<Guid>(Error.Validation("PhoneEmpty", "Phone number required."));
        if (!command.TenantId.HasValue)
            return Result.Failure<Guid>(Error.Validation("TenantMissing", "Tenant id not resolved."));

        var tenantId = command.TenantId.Value;

        // Ensure parent profile exists in tenant retail DB
        var retailDb = await retailFactory.CreateAsync(tenantId, ct);
        var parentProfile = await retailDb.TenantCustomerProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.GlobalCustomerId == command.ParentGlobalCustomerId, ct);
        if (parentProfile == null)
            return Result.Failure<Guid>(Error.NotFound("ParentProfileNotFound", "Parent profile not found in tenant."));

        if (parentProfile.Depth >= 5)
            return Result.Failure<Guid>(Error.Validation("DepthLimit", "Maximum hierarchy depth reached."));

        var phone = command.PhoneNumber.Trim();
        var email = $"{phone}@DandD.com";
        var pwd = $"{phone}@DandD";

        


        // Identity user
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phone, ct);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = phone,
                PhoneNumber = phone,
                Email = email,
                PhoneNumberConfirmed = true
            };
            var createUser = await userManager.CreateAsync(user, pwd);
            if (!createUser.Succeeded)
                return Result.Failure<Guid>(Error.Problem("UserCreateFailed", string.Join(',', createUser.Errors.Select(e => e.Description))));
        }

        // Central customer account
        var account = await authDb.CustomerAccounts
            .SingleOrDefaultAsync(c => c.GlobalUserId == user.Id, ct);
        bool newAccount = account == null;
        if (newAccount)
        {
            account = new CustomerAccount
            {
                GlobalUserId = user.Id,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = phone,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Active"
            };
            authDb.CustomerAccounts.Add(account);
            await authDb.SaveChangesAsync(ct); // need GlobalCustomerId for hierarchy path
        }
        else
        {
            // TODO : lets not update the customer name here in central db.

            //account.FirstName = command.FirstName;
            //account.LastName = command.LastName;
            //account.PhoneNumber = phone;
            //account.Email = email;
            //account.UpdatedAt = DateTime.UtcNow;
            //authDb.CustomerAccounts.Update(account);
            //await authDb.SaveChangesAsync(ct);
        }

        var childGlobalId = account.GlobalCustomerId;

        // Link to tenant centrally if not yet
        var linkExists = await authDb.CustomerTenantLinks
            .AnyAsync(l => l.GlobalCustomerId == childGlobalId && l.TenantId == tenantId, ct);
        if (!linkExists)
        {
            authDb.CustomerTenantLinks.Add(new CustomerTenantLink
            {
                GlobalCustomerId = childGlobalId,
                TenantId = tenantId,

            });
            await authDb.SaveChangesAsync(ct);
        }

        // Create tenant profile with hierarchical path
        var profileExists = await retailDb.TenantCustomerProfiles
            .AnyAsync(p => p.GlobalCustomerId == childGlobalId, ct);
        if (!profileExists)
        {
            var parentHierarchyPath = parentProfile.HierarchyPath;
            if (string.IsNullOrEmpty(parentHierarchyPath))
            {
                parentHierarchyPath = parentProfile.GlobalCustomerId.ToString();
            }
            var path = parentProfile.HierarchyPath + childGlobalId + "|";
            var depth = (byte)(parentProfile.Depth + 1);
            if (depth > 5)
                return Result.Failure<Guid>(Error.Validation("DepthLimit", "Hierarchy depth exceeds maximum (5)."));

            retailDb.TenantCustomerProfiles.Add(new TenantCustomerProfile
            {
                GlobalCustomerId = childGlobalId,
                TenantId = tenantId,
                ParentGlobalCustomerId = parentProfile.GlobalCustomerId,
                HierarchyPath = path,
                Depth = depth,
                DisplayName = string.Join(' ', new[] { command.FirstName, command.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DataKey = parentProfile.DataKey
            });
            await retailDb.SaveChangesAsync(ct);
        }

        return Result.Success(childGlobalId);
    }
}
