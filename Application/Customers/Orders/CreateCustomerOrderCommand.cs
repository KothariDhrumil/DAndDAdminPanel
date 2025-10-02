using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Orders;

public sealed class CreateCustomerOrderCommand : ICommand<int>
{
    public int? TenantId { get; set; }
    public string GlobalCustomerId { get; set; } = string.Empty;
    public decimal Total { get; set; }
}

internal sealed class CreateCustomerOrderCommandHandler(
    ITenantRetailDbContextFactory factory)
    : ICommandHandler<CreateCustomerOrderCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerOrderCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.GlobalCustomerId))
            return Result.Failure<int>(Error.Validation("CustomerIdEmpty", "Global customer id required."));
        if (command.Total <= 0)
            return Result.Failure<int>(Error.Validation("TotalInvalid", "Total must be > 0."));

        var tenantId = command.TenantId;
        if (!command.TenantId.HasValue)
            return Result.Failure<int>(Error.Validation("TenantMissing", "Tenant id not resolved."));

        var db = await factory.CreateAsync(command.TenantId.Value, ct);

        var order = new Order
        {
            GlobalCustomerId = command.GlobalCustomerId,
            OrderedAt = DateTime.UtcNow,
            Total = command.Total
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        return Result.Success(order.Id);
    }
}
