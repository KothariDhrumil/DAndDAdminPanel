using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders;

public sealed class GetDeliveredCustomerOrdersQuery : IQuery<PagedResult<List<CustomerOrderItemDto>>>
{
    public int? TenantId { get; set; }
    public int? RouteId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

internal sealed class GetDeliveredCustomerOrdersQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetDeliveredCustomerOrdersQuery, PagedResult<List<CustomerOrderItemDto>>>
{
    public async Task<Result<PagedResult<List<CustomerOrderItemDto>>>> Handle(GetDeliveredCustomerOrdersQuery query, CancellationToken ct)
    {
        if (!query.TenantId.HasValue)
            return Result.Success(PagedResult<List<CustomerOrderItemDto>>.Success(new List<CustomerOrderItemDto>(), query.Page, query.PageSize, 0));

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
                Remarks = o.Remarks,
                InvoiceNumber = o.InvoiceNumber,
                ParcelCharge = o.ParcelCharge,
                IsPreOrder = o.IsPreOrder,
                PayerCustomerId = o.PayerCustomerId,
                Details = o.CustomerOrderDetails.Select(d => new CustomerOrderDetailItemDto
                {
                    ProductId = d.ProductId,
                    Qty = d.Qty,
                    Rate = d.Rate,
                    Amount = d.Amount
                }).ToList()
            })
            .ToListAsync(ct);

        var page = PagedResult<List<CustomerOrderItemDto>>.Success(items, query.Page, query.PageSize, total);
        return Result.Success(page);
    }
}
