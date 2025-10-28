using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Customers;
using FluentValidation;
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
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }

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
                FirstName = command.FirstName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public class InitializeFranchiseProfileCommandValidator : AbstractValidator<InitializeFranchiseProfileCommand>
{
    public InitializeFranchiseProfileCommandValidator()
    {
        RuleFor(x => x.TenantId).GreaterThan(0);
        RuleFor(x => x.GlobalCustomerId).NotEmpty();
        RuleFor(x => x.FirstName).MaximumLength(100);
        RuleFor(x => x.LastName).MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(20);
    }
}
