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
        var items = await db.CustomerOrders
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

        return Result.Success(items);
    }
}
