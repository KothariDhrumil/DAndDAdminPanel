using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Hierarchy;

public sealed record GetFranchiseOrdersQuery(int TenantId, Guid RootGlobalCustomerId) : IQuery<List<FranchiseOrderDto>>;

public sealed record FranchiseOrderDto(int OrderId, Guid TenantCustomerId, DateTime OrderedAt, decimal Total);

internal sealed class GetFranchiseOrdersQueryHandler(ITenantRetailDbContextFactory factory)
    : IQueryHandler<GetFranchiseOrdersQuery, List<FranchiseOrderDto>>
{
    public async Task<Result<List<FranchiseOrderDto>>> Handle(GetFranchiseOrdersQuery query, CancellationToken ct)
    {
        var db = await factory.CreateAsync(query.TenantId, ct);

        // Get root profile
        var root = await db.TenantCustomerProfiles
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.GlobalCustomerId == query.RootGlobalCustomerId, ct);
        if (root == null)
            return SharedKernel.Result.Success(new List<FranchiseOrderDto>()); // empty

        var pathPrefix = root.HierarchyPath;

        // descendant ids as int to match Orders.TenantCustomerId type
        List<Guid> descendantIds = await db.TenantCustomerProfiles
            .AsNoTracking()
            .Where(p => EF.Functions.Like(p.HierarchyPath, pathPrefix + "%"))
            .Select(p => p.TenantUserId)
            .ToListAsync(ct);

        if (descendantIds.Count == 0)
            return SharedKernel.Result.Success(new List<FranchiseOrderDto>()); // empty

        var orders = await db.Orders.AsNoTracking()
            .Where(o => descendantIds.Contains(o.CustomerId))
            .Select(o => new FranchiseOrderDto(o.Id, o.CustomerId, o.OrderedAt, o.Total))
            .OrderByDescending(o => o.OrderedAt)
            .ToListAsync(ct);

        return SharedKernel.Result.Success(orders);
    }
}
