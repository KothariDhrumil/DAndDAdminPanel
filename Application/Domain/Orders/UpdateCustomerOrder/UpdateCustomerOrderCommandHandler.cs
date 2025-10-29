using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Services.CustomerOrderPriceCalculation;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Text;

namespace Application.Domain.Orders.UpdateCustomerOrder;

internal sealed class UpdateCustomerOrderCommandHandler(
    IRetailDbContext db,
    ICustomerOrderPriceCalculationService customerOrderPriceCalculationService)
    : ICommandHandler<UpdateCustomerOrderCommand, bool>
{

    public async Task<Result<bool>> Handle(UpdateCustomerOrderCommand command, CancellationToken ct)
    { 

        var order = await db.CustomerOrders
            .Include(o => o.CustomerOrderDetails)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);
        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));

        order.CustomerId = command.CustomerId;
        order.Discount = command.Discount;
        order.Remarks = command.Remarks;
        order.ParcelCharge = command.ParcelCharge;
        order.IsPreOrder = command.IsPreOrder;
        if (command.OrderDeliveryDate.HasValue)
            order.OrderDeliveryDate = command.OrderDeliveryDate.Value;
        if (command.IsDelivered.HasValue)
            order.IsDelivered = command.IsDelivered.Value;

        // Update details: simple replace
        var existing = order.CustomerOrderDetails.ToList();

        foreach (var item in command.CustomerOrderDetails)
        {
            var detail = existing.FirstOrDefault(d => d.ProductId == item.ProductId);
            if (detail == null)
            {
                order.CustomerOrderDetails.Add(new CustomerOrderDetail
                {
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                });
            }
            else
            {
                detail.Qty = item.Qty;
            }
        }

        foreach (var existingDetail in existing)
        {
            if (!command.CustomerOrderDetails.Any(x => x.ProductId == existingDetail.ProductId) ||
                command.CustomerOrderDetails.FirstOrDefault(x => x.ProductId == existingDetail.ProductId)?.Qty == 0)
                order.CustomerOrderDetails.Remove(existingDetail);
        }
        await customerOrderPriceCalculationService.SaveOrUpdateOrderAsync(order);

        await db.SaveChangesAsync(ct);
        return Result.Success(true);
    }
}
