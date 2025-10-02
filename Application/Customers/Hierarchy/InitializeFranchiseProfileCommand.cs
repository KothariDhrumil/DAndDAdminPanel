using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Hierarchy;

/// <summary>
/// Ensures a TenantCustomerProfile exists with a root hierarchy path for a given customer in a tenant.
/// If already present, returns success (idempotent). Does not alter existing path.
/// </summary>
public sealed class InitializeFranchiseProfileCommand : ICommand
{
    public int TenantId { get; set; }
    public Guid GlobalCustomerId { get; set; }
    public string? DisplayName { get; set; }

    internal sealed class Handler(ITenantRetailDbContextFactory factory)
        : ICommandHandler<InitializeFranchiseProfileCommand>
    {
        public async Task<Result> Handle(InitializeFranchiseProfileCommand command, CancellationToken ct)
        {
            var db = await factory.CreateAsync(command.TenantId, ct);

            var existing = await db.TenantCustomerProfiles
                .SingleOrDefaultAsync(p => p.GlobalCustomerId == command.GlobalCustomerId, ct);
            if (existing != null)
                return Result.Success();

            db.TenantCustomerProfiles.Add(new TenantCustomerProfile
            {
                GlobalCustomerId = command.GlobalCustomerId,
                TenantId = command.TenantId,
                ParentGlobalCustomerId = null,
                HierarchyPath = "|" + command.GlobalCustomerId + "|",
                Depth = 1, // root depth =1 (or 0 if you prefer; adjust SetFranchiseParent accordingly)
                DisplayName = command.DisplayName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
