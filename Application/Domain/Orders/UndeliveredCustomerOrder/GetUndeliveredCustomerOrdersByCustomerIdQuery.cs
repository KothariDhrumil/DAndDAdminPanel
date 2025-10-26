using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders.UndeliveredCustomerOrder;

public sealed class GetUndeliveredCustomerOrdersByCustomerIdQuery : IQuery<UndeliveredCustomerOrderItemDto>
{
    public Guid CustomerId { get; set; }
}

internal sealed class GetUndeliveredCustomerOrdersByCustomerIdQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetUndeliveredCustomerOrdersByCustomerIdQuery, UndeliveredCustomerOrderItemDto>
{
    public async Task<Result<UndeliveredCustomerOrderItemDto>> Handle(GetUndeliveredCustomerOrdersByCustomerIdQuery query, CancellationToken ct)
    {
        var order = await db.CustomerOrders
            .AsNoTracking()
            .Where(o => o.IsDelivered == false && o.CustomerId == query.CustomerId)
            .Select(o => new UndeliveredCustomerOrderItemDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderPlacedDate = o.OrderPlacedDate,
                OrderDeliveryDate = o.OrderDeliveryDate,
                Amount = o.Amount,
                Discount = o.Discount,
                Remarks = o.Remarks,
                ParcelCharge = o.ParcelCharge,
                CustomerOrderDetails = o.CustomerOrderDetails.Select(d => new UndeliveredCustomerOrderDetailItemDto
                {
                    ProductId = d.ProductId,
                    Qty = d.Qty,
                    SalesRate = d.Rate,
                    ProductName = d.Product.Name
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        // If no undelivered order, return null DTO
        if (order == null)
            return Result.Success<UndeliveredCustomerOrderItemDto>(null);

        var existingProductIds = order.CustomerOrderDetails.Select(d => d.ProductId).ToHashSet();

        var missingProducts = await db.CustomerProducts
            .AsNoTracking()
            .Where(cp => cp.CustomerId == query.CustomerId
                && cp.Product.IsActive
                && !existingProductIds.Contains(cp.ProductId))
            .Select(cp => new UndeliveredCustomerOrderDetailItemDto
            {
                ProductId = cp.ProductId,
                Qty = 0,
                SalesRate = cp.SalesRate,
                ProductName = cp.Product.Name
            })
            .ToListAsync(ct);

        order.CustomerOrderDetails.AddRange(missingProducts);

        return Result.Success(order);
    }
}
