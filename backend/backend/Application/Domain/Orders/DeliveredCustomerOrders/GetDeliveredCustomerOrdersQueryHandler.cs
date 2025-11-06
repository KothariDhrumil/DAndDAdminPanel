using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders.DeliveredCustomerOrders;

internal sealed class GetDeliveredCustomerOrdersQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetDeliveredCustomerOrdersQuery, PagedResult<List<CustomerOrderItemDto>>>
{
    public async Task<Result<PagedResult<List<CustomerOrderItemDto>>>> Handle(GetDeliveredCustomerOrdersQuery query, CancellationToken ct)
    {

        var orders = db.CustomerOrders.AsNoTracking().Where(o => o.IsDelivered == true);

        if (query.RouteId.HasValue)
            orders = orders.Where(o => o.Customer.RouteId == query.RouteId.Value);
        if (query.FromDate.HasValue)
            orders = orders.Where(o => o.OrderDeliveryDate >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            orders = orders.Where(o => o.OrderDeliveryDate <= query.ToDate.Value);

        var total = await orders.CountAsync(ct);

        var items = await orders
            .OrderByDescending(o => o.OrderDeliveryDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
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
                ParcelCharge = o.ParcelCharge,
                IsPreOrder = o.IsPreOrder,
                CustomerName = o.Customer.FirstName + ' ' + o.Customer.LastName,
                RouteName = o.Customer.Route.Name,
                CreatedBy = o.CreatedBy,
               
            })
            .ToListAsync(ct);

        var page = PagedResult<List<CustomerOrderItemDto>>.Success(items, query.Page, query.PageSize, total);
        return Result.Success(page);
        
    }
}
