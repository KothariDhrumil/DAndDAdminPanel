using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders;

public sealed class DeleteCustomerOrderCommand : ICommand<bool>
{
    public int OrderId { get; set; }
}

internal sealed class DeleteCustomerOrderCommandHandler(
    IRetailDbContext db)
    : ICommandHandler<DeleteCustomerOrderCommand, bool>
{
    public async Task<Result<bool>> Handle(DeleteCustomerOrderCommand command, CancellationToken ct)
    {
        var order = await db.CustomerOrders
            .Include(o => o.CustomerOrderDetails)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);
        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));
        if (order.IsDelivered)
            return Result.Failure<bool>(Error.Validation("OrderDelivered", "Cannot delete a delivered order."));
        
        if (order.CustomerOrderDetails.Any())
            db.CustomerOrderDetails.RemoveRange(order.CustomerOrderDetails);

        db.CustomerOrders.Remove(order);
        await db.SaveChangesAsync(ct);
        return Result.Success(true);
    }
}
