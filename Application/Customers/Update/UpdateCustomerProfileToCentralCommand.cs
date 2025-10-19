using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Types;
using SharedKernel;

namespace Application.Customers.Update;

public sealed class UpdateCustomerProfileToCentralCommand : ICommand
{
    public Guid GlobalCustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    public string Address { get; set; } = string.Empty;
    public double OpeningBalance { get; set; }
    public bool IsActive { get; set; }
    public bool TaxExempt { get; set; }
    public bool CourierChargesApplicable { get; set; }

    public string GSTNumber { get; set; } = string.Empty;
    public string GSTName { get; set; }
    public double CreditLimit { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext authDb)
        : ICommandHandler<UpdateCustomerProfileToCentralCommand>
    {
        public async Task<Result> Handle(UpdateCustomerProfileToCentralCommand command, CancellationToken ct)
        {
            var account = await authDb.CustomerAccounts
                .SingleOrDefaultAsync(c => c.GlobalCustomerId == command.GlobalCustomerId, ct);
            if (account == null)
                return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));

            account.FirstName = command.FirstName;
            account.LastName = command.LastName;
            if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
                account.PhoneNumber = command.PhoneNumber.Trim();
            authDb.CustomerAccounts.Update(account);

            await authDb.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerProfileToCentralCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.GlobalCustomerId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(128);
        RuleFor(x => x.PhoneNumber).Length(10, 32).When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
    }
}
