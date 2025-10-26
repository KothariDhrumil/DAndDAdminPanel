using Application.Abstractions.Messaging;
using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders.DeliverOrder;

internal sealed class DeliverExistingOrderCommandHandler(
    IRetailDbContext db)
    : ICommandHandler<DeliverExistingOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(DeliverExistingOrderCommand command, CancellationToken ct)
    {
        var order = await db.CustomerOrders.FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);
        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));
        order.IsDelivered = true;
        order.OrderDeliveryDate = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success(true);
    }
}
