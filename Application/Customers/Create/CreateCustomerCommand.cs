using Application.Abstractions.Messaging;
using Application.Customers.Services;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Create;

public sealed class CreateCustomerCommand : ICommand<Guid>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? TenantId { get; set; }

    internal sealed class Handler(
        ICustomerOnboardingService provisioning)
        : ICommandHandler<CreateCustomerCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateCustomerCommand command, CancellationToken ct)
        {
            var phone = command.PhoneNumber?.Trim();
            
            var user = await provisioning.EnsureUserAsync(phone, command.FirstName, command.LastName, ct);
            var globalCustomerId = await provisioning.EnsureCustomerAccountAsync(user, phone, user.Email, command.FirstName, command.LastName, ct);

            if (command.TenantId.HasValue)
            {
                var display = string.Join(' ', new[] { command.FirstName, command.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                await provisioning.EnsureLinkedToTenantAsync(globalCustomerId, command.TenantId.Value, display, ct);
            }
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
