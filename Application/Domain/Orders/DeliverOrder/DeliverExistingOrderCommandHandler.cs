using Application.Abstractions.Messaging;
using Application.Abstractions.Data;
using Application.Abstractions.Authentication;
using Application.Services.Orders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders.DeliverOrder;

internal sealed class DeliverExistingOrderCommandHandler(
    IRetailDbContext db,
    IOrderDeliveryService orderDeliveryService,
    IUserContext userContext)
    : ICommandHandler<DeliverExistingOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(DeliverExistingOrderCommand command, CancellationToken ct)
    {
        var order = await db.CustomerOrders
            .Include(o => o.CustomerOrderDetails)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);
        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));
        
        order.IsDelivered = true;
        order.OrderDeliveryDate = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        
        // Handle post-delivery accounting
        await orderDeliveryService.HandlePostDeliveryAsync(order, userContext.UserId, ct);
        
        return Result.Success(true);
    }
}
