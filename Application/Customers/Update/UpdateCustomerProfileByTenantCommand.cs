using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Update
{

    public sealed class UpdateCustomerProfileByTenantCommand : ICommand
    {

        public Guid GlobalCustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public double OpeningBalance { get; set; }
        public bool IsActive { get; set; }
        public bool TaxEnabled { get; set; }
        public bool CourierChargesApplicable { get; set; }
        public string GSTNumber { get; set; } = string.Empty;
        public string GSTName { get; set; } = string.Empty;
        public double CreditLimit { get; set; }

        internal sealed class Handler(IRetailDbContext retailDbContext)
      : ICommandHandler<UpdateCustomerProfileByTenantCommand>
        {
            public async Task<Result> Handle(UpdateCustomerProfileByTenantCommand command, CancellationToken ct)
            {
                var account = await retailDbContext.TenantCustomerProfiles
                    .SingleOrDefaultAsync(c => c.GlobalCustomerId == command.GlobalCustomerId, ct);
                if (account == null)
                    return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));

                account.FirstName = command.FirstName;
                account.LastName = command.LastName;
                account.Address = command.Address;
                account.OpeningBalance = account.OpeningBalance == 0 ? command.OpeningBalance : account.OpeningBalance;
                account.IsActive = command.IsActive;
                account.TaxEnabled = command.TaxEnabled;
                account.CourierChargesApplicable = command.CourierChargesApplicable;
                account.GSTNumber = command.GSTNumber;
                account.GSTName = command.GSTName;
                account.CreditLimit = command.CreditLimit;
                retailDbContext.TenantCustomerProfiles.Update(account);
                await retailDbContext.SaveChangesAsync(ct);
                return Result.Success();
            }
        }
    }

    public sealed class UpdateCustomerProfileByTenantCommandValidator : AbstractValidator<UpdateCustomerProfileByTenantCommand>
    {
        public UpdateCustomerProfileByTenantCommandValidator()
        {
            RuleFor(x => x.GlobalCustomerId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(128);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(128);
            RuleFor(x => x.PhoneNumber).Length(10, 32).When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        }
    }

}
