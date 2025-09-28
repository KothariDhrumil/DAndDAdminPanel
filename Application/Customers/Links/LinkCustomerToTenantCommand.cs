using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Application.Abstractions.Persistence;
using Domain.Customers;

namespace Application.Customers.Links;

public sealed class LinkCustomerToTenantCommand : ICommand
{
    public required Guid GlobalCustomerId { get; set; }
    public int TenantId { get; set; }

    internal sealed class Handler(
        AuthPermissionsDbContext authDb,
        ITenantRetailDbContextFactory retailFactory)
        : ICommandHandler<LinkCustomerToTenantCommand>
    {
        public async Task<Result> Handle(LinkCustomerToTenantCommand command, CancellationToken ct)
        {
            var account = await authDb.CustomerAccounts.AsNoTracking()
                .SingleOrDefaultAsync(c => c.GlobalCustomerId == command.GlobalCustomerId, ct);
            if (account == null)
                return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));

            // link in central db
            var exists = await authDb.CustomerTenantLinks
                .AnyAsync(l => l.GlobalCustomerId == account.GlobalCustomerId && l.TenantId == command.TenantId, ct);
            if (!exists)
            {
                authDb.CustomerTenantLinks.Add(new CustomerTenantLink
                {
                    GlobalCustomerId = account.GlobalCustomerId,
                    TenantId = command.TenantId
                });
                await authDb.SaveChangesAsync(ct);
            }

            // ensure tenant profile exists
            var retailDb = await retailFactory.CreateAsync(command.TenantId, ct);
            var hasProfile = await retailDb.TenantCustomerProfiles
                .AnyAsync(p => p.GlobalCustomerId == account.GlobalCustomerId, ct);
            if (!hasProfile)
            {
                retailDb.TenantCustomerProfiles.Add(new TenantCustomerProfile
                {
                    GlobalCustomerId = account.GlobalCustomerId,
                    TenantId = command.TenantId,
                    DisplayName = string.Join(' ', new[] { account.FirstName, account.LastName }.Where(s => !string.IsNullOrWhiteSpace(s))),
                    DataKey = retailDb.DataKey
                });
                await retailDb.SaveChangesAsync(ct);
            }

            return Result.Success();
        }
    }
}

public sealed class LinkCustomerToTenantCommandValidator : AbstractValidator<LinkCustomerToTenantCommand>
{
    public LinkCustomerToTenantCommandValidator()
    {
        RuleFor(x => x.GlobalCustomerId).NotEmpty();
        RuleFor(x => x.TenantId).GreaterThan(0);
    }
}
