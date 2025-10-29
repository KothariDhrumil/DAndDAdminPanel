using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Authentication;
using Application.Services.Orders;
using Domain.Purchase;
using SharedKernel;
using Application.Services.CustomerOrderPriceCalculation;

namespace Application.Domain.Orders.DeliverOrder;

internal sealed class DeliverDirectSalesOrderCommandHandler(
    IRetailDbContext db,
    ICustomerOrderPriceCalculationService customerOrderPriceCalculationService,
    IOrderDeliveryService orderDeliveryService,
    IUserContext userContext)
    : ICommandHandler<DeliverDirectSalesOrderCommand, int>
{
    public async Task<Result<int>> Handle(DeliverDirectSalesOrderCommand command, CancellationToken ct)
    {
        if (command.CustomerOrderDetails == null || !command.CustomerOrderDetails.Any())
            return Result.Failure<int>(Error.Validation("OrderDetailsMissing", "Order must have at least one detail."));
        
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var order = new CustomerOrder
        {
            CustomerId = command.CustomerId,
            RouteId = command.RouteId,
            Discount = command.Discount,
            Remarks = command.Remarks,
            ParcelCharge = command.ParcelCharge,
            IsPreOrder = command.IsPreOrder,
            OrderPlacedDate = DateTime.UtcNow,
            OrderDeliveryDate = DateTime.UtcNow,
            IsDelivered = true,
            CustomerOrderDetails = command.CustomerOrderDetails.Select(d => new CustomerOrderDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,                
            }).ToList()
        };
        await customerOrderPriceCalculationService.SaveOrUpdateOrderAsync(order);

        db.CustomerOrders.Add(order);

        await db.SaveChangesAsync(ct);
        
        // Handle post-delivery accounting
        await orderDeliveryService.HandlePostDeliveryAsync(order, userContext.UserId, ct);

        // Step 5: Save and commit transaction
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Result.Success(order.Id);
    }
}
