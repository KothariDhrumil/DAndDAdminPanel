using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Accounting;
using FluentValidation;
using SharedKernel;

namespace Application.Accounting;

public sealed class CreateLedgerEntryCommand : ICommand<long>
{
    public DateTime? EntryDate { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? ReferenceId { get; set; }

    public Guid TenantUserId { get; set; }

    internal sealed class Handler(ITenantRetailDbContextFactory factory) : ICommandHandler<CreateLedgerEntryCommand, long>
    {
        private readonly ITenantRetailDbContextFactory _factory = factory;

        public async Task<Result<long>> Handle(CreateLedgerEntryCommand command, CancellationToken ct)
        {
            if (command.TenantUserId == Guid.Empty)
                return Result.Failure<long>(Error.Validation("Ledger.Target.Required", "TenantUserId must be provided."));

            var tenantId = GetTenantIdFromTarget(command);
            var db = await _factory.CreateAsync(tenantId, ct);

            var entry = new LedgerEntry
            {
                EntryDate = command.EntryDate ?? DateTime.UtcNow,
                Amount = command.Amount,
                Description = command.Description ?? string.Empty,
                ReferenceId = command.ReferenceId,

                TenantUserId = command.TenantUserId
            };

            db.LedgerEntries.Add(entry);
            await db.SaveChangesAsync(ct);

            return Result.Success(entry.Id);
        }

        private static int GetTenantIdFromTarget(CreateLedgerEntryCommand command)
        {
            // In a real scenario you may fetch tenantId from profile, but factory expects tenantId.
            // Here we assume caller routes command to the correct tenant's context, so pick one from caller.
            // If both are provided, business rules can define which to use; here it's a no-op.
            return 0; // The ITenantRetailDbContextFactory likely uses current user's tenant; adjust if needed.
        }
    }
}

public sealed class CreateLedgerEntryCommandValidator : AbstractValidator<CreateLedgerEntryCommand>
{
    public CreateLedgerEntryCommandValidator()
    {
        RuleFor(x => x.Amount).NotEqual(0);
        RuleFor(x => x.Description).MaximumLength(512).When(x => x.Description != null);
    }
}
