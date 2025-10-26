using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Pricing;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Orders.DeliverOrder;

internal sealed class DeliverDirectSalesOrderCommandHandler(
    IRetailDbContext db, ICustomerOrderPriceCalculationService customerOrderPriceCalculationService)
    : ICommandHandler<DeliverDirectSalesOrderCommand, int>
{
    public async Task<Result<int>> Handle(DeliverDirectSalesOrderCommand command, CancellationToken ct)
    {
        if (command.CustomerOrderDetails == null || !command.CustomerOrderDetails.Any())
            return Result.Failure<int>(Error.Validation("OrderDetailsMissing", "Order must have at least one detail."));
        var order = new CustomerOrder
        {
            CustomerId = command.CustomerId,
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
        return Result.Success(order.Id);
    }
}
