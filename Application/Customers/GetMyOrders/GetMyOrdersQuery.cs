using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.GetMyOrders;

public sealed record GetMyOrdersQuery(string CustomerUserId) : IQuery<List<MyOrderDto>>;

public sealed class MyOrderDto
{
    public int TenantId { get; set; }
    public string TenantName { get; set; }
    public int OrderId { get; set; }
    public DateTime OrderedAt { get; set; }
    public decimal Total { get; set; }
}

internal sealed class GetMyOrdersQueryHandler(
    AuthPermissionsDbContext authContext,
    ITenantRetailDbContextFactory retailFactory)
    : IQueryHandler<GetMyOrdersQuery, List<MyOrderDto>>
{
    public async Task<SharedKernel.Result<List<MyOrderDto>>> Handle(GetMyOrdersQuery query, CancellationToken ct)
    {
        var links = await authContext.CustomerTenantLinks
            .Join(authContext.Tenants,
                (CustomerTenantLink l) => l.TenantId,
                (Tenant t) => t.TenantId,
                (l, t) => new { l, t })
            .Where(x => true) // already filtered by user before? optional
            .Select(x => new { x.l.TenantId, x.t.TenantFullName })
            .ToListAsync(ct);

        var list = new List<MyOrderDto>();

        foreach (var link in links)
        {
            var db = await retailFactory.CreateAsync(link.TenantId, ct);
            var orders = await db.Orders.AsNoTracking()
                .Where(o => o.GlobalCustomerId == query.CustomerUserId)
                .Select(o => new MyOrderDto
                {
                    TenantId = link.TenantId,
                    TenantName = link.TenantFullName,
                    OrderId = o.Id,
                    OrderedAt = o.OrderedAt,
                    Total = o.Total
                })
                .ToListAsync(ct);
            list.AddRange(orders);
        }
        list = list.OrderByDescending(x => x.OrderedAt).ToList();
        return SharedKernel.Result.Success(list);
    }
}
