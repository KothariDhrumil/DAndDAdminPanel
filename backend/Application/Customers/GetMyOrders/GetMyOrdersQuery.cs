using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.GetMyOrders;

public sealed record GetMyOrdersQuery(Guid GlobalCustomerId, int? tenantId) : IQuery<List<MyOrderDto>>;

public sealed class MyOrderDto
{
    public int TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
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
        if (!query.tenantId.HasValue)
        {
            var links = await authContext.CustomerTenantLinks
          .Join(authContext.Tenants,
              (CustomerTenantLink l) => l.TenantId,
              (Tenant t) => t.TenantId,
              (l, t) => new { l, t })
          .Where(x => x.l.GlobalCustomerId == query.GlobalCustomerId)
          .Select(x => new { x.l.TenantId, x.t.TenantFullName, x.l.GlobalCustomerId })
          .ToListAsync(ct);


            var list = new List<MyOrderDto>();

            foreach (var link in links)
            {
                var db = await retailFactory.CreateAsync(link.TenantId, ct);
                var profileId = await db.TenantCustomerProfiles.AsNoTracking()
                    .Where(p => p.GlobalCustomerId == link.GlobalCustomerId)
                    .Select(p => p.TenantUserId)
                    .SingleOrDefaultAsync(ct);
                if (profileId == Guid.Empty) continue;

                var orders = await db.Orders.AsNoTracking()
                    .Where(o => o.CustomerId == profileId)
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
        else
        {
            var db = await retailFactory.CreateAsync(query.tenantId.Value, ct);
            var profileId = await db.TenantCustomerProfiles.AsNoTracking()
                .Where(p => p.GlobalCustomerId == query.GlobalCustomerId)
                .Select(p => p.TenantUserId)
                .SingleOrDefaultAsync(ct);
            if (profileId == Guid.Empty)
                return SharedKernel.Result.Failure<List<MyOrderDto>>(SharedKernel.Error.NotFound("ProfileNotFound", "Customer profile not found in tenant."));
       
            var orders = await db.Orders.AsNoTracking()
                .Where(o => o.CustomerId == profileId)
                .Select(o => new MyOrderDto
                {
                    TenantId = query.tenantId.Value,
                    OrderId = o.Id,
                    OrderedAt = o.OrderedAt,
                    Total = o.Total
                })
                .OrderByDescending(o => o.OrderedAt)
                .ToListAsync(ct);
            return SharedKernel.Result.Success(orders);
        }
    }
}