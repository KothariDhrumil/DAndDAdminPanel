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
        var duplicateProducts = command.CustomerOrderDetails
                                        .GroupBy(i => i.ProductId)
                                        .Where(g => g.Count() > 1)
                                        .Select(g => g.Key)
                                        .ToList();

        if (duplicateProducts.Any())
            return Result.Failure<bool>(
                Error.Validation("DuplicateProducts", $"Duplicate products found: {string.Join(", ", duplicateProducts)}"));


        var order = await db.CustomerOrders
            .Include(o => o.CustomerOrderDetails)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, ct);

        var incomingByProduct = command.CustomerOrderDetails
            .Where(i => i.Qty > 0) // treat zero as deletion
            .ToDictionary(i => i.ProductId, i => i);

        if (order == null)
            return Result.Failure<bool>(Error.NotFound("OrderNotFound", "Order not found."));

        order.CustomerId = command.CustomerId;
        order.Discount = command.Discount;
        order.Remarks = command.Remarks;
        order.ParcelCharge = command.ParcelCharge;
        order.IsPreOrder = command.IsPreOrder;
        //if (command.OrderDeliveryDate.HasValue)
        //    order.OrderDeliveryDate = command.OrderDeliveryDate.Value;
        //if (command.IsDelivered.HasValue)
        //    order.IsDelivered = command.IsDelivered.Value;



        // Update details: simple replace
        var existingList = order.CustomerOrderDetails.ToList();
        var existingByProduct = existingList.ToDictionary(x => x.ProductId);


        // 1) Delete: existing products not in incoming
        var toRemove = existingList.Where(e => !incomingByProduct.ContainsKey(e.ProductId)).ToList();
        foreach (var rem in toRemove)
        {
            // explicit deletion
            db.CustomerOrderDetails.Remove(rem); // or db.Set<CustomerOrderDetail>().Remove(rem);
        }

        foreach (var item in incomingByProduct)
        {
            if (!existingByProduct.TryGetValue(item.Key, out var detail))
            {
                order.CustomerOrderDetails.Add(new CustomerOrderDetail
                {
                    ProductId = item.Key,
                    Qty = item.Value.Qty,
                });
            }
            else
            {
                detail.Qty = item.Value.Qty;
            }
        }

       
        await customerOrderPriceCalculationService.ApplyPricingAsync(order);

        await db.SaveChangesAsync(ct);
        return Result.Success(true);
    }
}
