using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Links;

public sealed class UnlinkCustomerFromTenantCommand : ICommand
{
    public string PhoneNumber { get; set; } = string.Empty;
    public int TenantId { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext authDb)
        : ICommandHandler<UnlinkCustomerFromTenantCommand>
    {
        public async Task<Result> Handle(UnlinkCustomerFromTenantCommand command, CancellationToken ct)
        {
            var phone = command.PhoneNumber.Trim();

            var account = await authDb.CustomerAccounts.AsNoTracking()
                .SingleOrDefaultAsync(c => c.PhoneNumber == phone, ct);
            if (account == null)
                return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));

            var link = await authDb.CustomerTenantLinks
                .SingleOrDefaultAsync(l => l.GlobalCustomerId == account.GlobalCustomerId && l.TenantId == command.TenantId, ct);
            if (link == null)
                return Result.Success(); // idempotent

            authDb.CustomerTenantLinks.Remove(link);
            await authDb.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public sealed class UnlinkCustomerFromTenantCommandValidator : AbstractValidator<UnlinkCustomerFromTenantCommand>
{
    public UnlinkCustomerFromTenantCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty().Length(10, 32);
        RuleFor(x => x.TenantId).GreaterThan(0);
    }
}
