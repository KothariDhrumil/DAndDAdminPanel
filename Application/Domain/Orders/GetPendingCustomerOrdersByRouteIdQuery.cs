using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders;

public sealed class GetPendingCustomerOrdersByRouteIdQuery : IQuery<List<CustomerOrderItemDto>>
{
    public int? RouteId { get; set; }
}

internal sealed class GetPendingCustomerOrdersByRouteIdQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetPendingCustomerOrdersByRouteIdQuery, List<CustomerOrderItemDto>>
{
    public async Task<Result<List<CustomerOrderItemDto>>> Handle(GetPendingCustomerOrdersByRouteIdQuery query, CancellationToken ct)
    {

        var command = db.CustomerOrders
            .AsNoTracking()
            .Where(o => o.IsDelivered == false);

        if (query.RouteId.HasValue)
            command = command.Where(o => o.Customer.RouteId == query.RouteId.Value);

        var items = await command
            .Select(o => new CustomerOrderItemDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderPlacedDate = o.OrderPlacedDate,
                OrderDeliveryDate = o.OrderDeliveryDate,
                IsDelivered = o.IsDelivered,
                Amount = o.Amount,
                Discount = o.Discount,
                Tax = o.Tax,
                GrandTotal = o.GrandTotal,
                Remarks = o.Remarks,
                InvoiceNumber = o.InvoiceNumber,
                ParcelCharge = o.ParcelCharge,
                IsPreOrder = o.IsPreOrder,
                PayerCustomerId = o.PayerCustomerId,
                CustomerOrderDetails = o.CustomerOrderDetails.Select(d => new CustomerOrderDetailItemDto
                {
                    ProductId = d.ProductId,
                    Qty = d.Qty,
                    Rate = d.Rate,
                    Amount = d.Amount
                }).ToList()
            })
            .ToListAsync(ct);

        return Result.Success(items);
    }
}
