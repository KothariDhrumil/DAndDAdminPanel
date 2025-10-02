using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Application.Abstractions.Persistence;
using Domain.Customers;

namespace Application.Customers.Create;

public sealed class CreateCustomerCommand : ICommand<Guid>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? TenantId { get; set; } // optional immediate mapping

    internal sealed class Handler(
        AuthPermissionsDbContext authDb,
        UserManager<ApplicationUser> userManager,
        ITenantRetailDbContextFactory retailFactory)
        : ICommandHandler<CreateCustomerCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken ct)
        {
            var phone = command.PhoneNumber?.Trim();
            var email = $"{phone}@DandD.com";
            var pwd = $"{phone}@DandD";

            // Find or create identity user
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
                var created = await userManager.CreateAsync(user, pwd);
                if (!created.Succeeded)
                    return Result.Failure<Guid>(Error.Problem("UserCreateFailed", string.Join(',', created.Errors.Select(e => e.Description))));
            }

            // Find or create customer account
            var existing = await authDb.CustomerAccounts
                .SingleOrDefaultAsync(c => c.GlobalUserId == user.Id, ct);

            Guid globalCustomerId;
            if (existing == null)
            {
                var account = new CustomerAccount
                {
                    //GlobalCustomerId = Guid.NewGuid(),
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
                await authDb.SaveChangesAsync(ct);
                globalCustomerId = account.GlobalCustomerId;
            }
            else
            {
                // TODO : lets not update the customer name here in central db.

                //existing.FirstName = command.FirstName;
                //existing.LastName = command.LastName;
                //existing.PhoneNumber = phone;
                //existing.Email = email;
                //existing.UpdatedAt = DateTime.UtcNow;
                //authDb.CustomerAccounts.Update(existing);

                globalCustomerId = existing.GlobalCustomerId;
            }

            // Optional mapping to tenant
            if (command.TenantId.HasValue)
            {
                var tenantId = command.TenantId.Value;
                var linkExists = await authDb.CustomerTenantLinks
                    .AnyAsync(l => l.GlobalCustomerId == globalCustomerId && l.TenantId == tenantId, ct);
                if (!linkExists)
                {
                    authDb.CustomerTenantLinks.Add(new CustomerTenantLink
                    {
                        GlobalCustomerId = globalCustomerId,
                        TenantId = tenantId
                    });
                }

                // Create per-tenant profile if missing
                var retailDb = await retailFactory.CreateAsync(tenantId, ct);
                var profileExists = await retailDb.TenantCustomerProfiles
                    .AnyAsync(p => p.GlobalCustomerId == globalCustomerId, ct);
                if (!profileExists)
                {
                    retailDb.TenantCustomerProfiles.Add(new TenantCustomerProfile
                    {
                        GlobalCustomerId = globalCustomerId,
                        TenantId = tenantId,
                        DisplayName = string.Join(' ', new[] { command.FirstName, command.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))),
                        DataKey = retailDb.DataKey
                    });
                    await retailDb.SaveChangesAsync(ct);
                }
            }
            await authDb.SaveChangesAsync(ct);
            return Result.Success(globalCustomerId);
        }
    }
}

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().Length(10, 32);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.TenantId).GreaterThan(0).When(x => x.TenantId.HasValue);
    }
}
