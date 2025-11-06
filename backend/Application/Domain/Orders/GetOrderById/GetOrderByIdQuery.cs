using Application.Abstractions.Messaging;
using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders.GetOrderById;

public sealed class GetOrderByIdQuery : IQuery<GetCustomerOrderByIdDto?>
{
    public int OrderId { get; set; }
}

internal sealed class GetOrderByIdQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetOrderByIdQuery, GetCustomerOrderByIdDto?>
{
    public async Task<Result<GetCustomerOrderByIdDto?>> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await db.CustomerOrders
            .Include(x=>x.CustomerOrderDetails)
            .AsNoTracking()
            .Where(o => o.Id == query.OrderId)
            .Select(o => new GetCustomerOrderByIdDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                Amount = o.Amount,
                Discount = o.Discount,
                GrandTotal = o.GrandTotal,
                Remarks = o.Remarks,
                ParcelCharge = o.ParcelCharge,
                IsPreOrder = o.IsPreOrder,
                CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                RouteId = o.Customer.RouteId,
                CustomerOrderDetails = o.CustomerOrderDetails.Select(d => new CustomerOrderDetailItemDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.Name,
                    Qty = d.Qty,
                    SalesRate = d.Rate,
                    Amount = d.Amount
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);
        return Result.Success(order);
    }
}
