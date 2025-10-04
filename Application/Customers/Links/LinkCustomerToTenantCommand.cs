using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Application.Abstractions.Persistence;
using Domain.Customers;
using Application.Customers.Services;

namespace Application.Customers.Links;

public sealed class LinkCustomerToTenantCommand : ICommand
{
    public required Guid GlobalCustomerId { get; set; }
    public int TenantId { get; set; }

    internal sealed class Handler(
        AuthPermissionsDbContext authDb,
        ICustomerOnboardingService provisioning)
        : ICommandHandler<LinkCustomerToTenantCommand>
    {
        public async Task<Result> Handle(LinkCustomerToTenantCommand command, CancellationToken ct)
        {
            var account = await authDb.CustomerAccounts.AsNoTracking()
                .SingleOrDefaultAsync(c => c.GlobalCustomerId == command.GlobalCustomerId, ct);
            if (account == null)
                return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));

            var display = string.Join(' ', new[] { account.FirstName, account.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
            await provisioning.EnsureLinkedToTenantAsync(account.GlobalCustomerId, command.TenantId, display, ct);

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
