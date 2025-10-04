using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Orders;

public sealed class CreateCustomerOrderCommand : ICommand<int>
{
    public int? TenantId { get; set; }
    public Guid GlobalCustomerId { get; set; }
    public decimal Total { get; set; }
}

internal sealed class CreateCustomerOrderCommandHandler(
    ITenantRetailDbContextFactory factory)
    : ICommandHandler<CreateCustomerOrderCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerOrderCommand command, CancellationToken ct)
    {
        if (command.Total <= 0)
            return Result.Failure<int>(Error.Validation("TotalInvalid", "Total must be > 0."));
        if (!command.TenantId.HasValue)
            return Result.Failure<int>(Error.Validation("TenantMissing", "Tenant id not resolved."));

        var db = await factory.CreateAsync(command.TenantId.Value, ct);

        // Resolve tenant-local profile
        var profile = await db.TenantCustomerProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.GlobalCustomerId == command.GlobalCustomerId, ct);
        if (profile == null)
            return Result.Failure<int>(Error.NotFound("ProfileNotFound", "Customer profile not found in tenant."));
        
        var order = new Order
        {
            TenantCustomerId = profile.TenantCustomerId,
            OrderedAt = DateTime.UtcNow,
            Total = command.Total,
            
        };
        
        order.SetDataKey(db.DataKey);

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);
        return Result.Success(order.Id);
    }
}
