using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Orders;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Hierarchy;

public sealed record GetFranchiseOrdersQuery(int TenantId, Guid RootGlobalCustomerId) : IQuery<List<FranchiseOrderDto>>;

public sealed record FranchiseOrderDto(int OrderId, string GlobalCustomerId, DateTime OrderedAt, decimal Total);

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
            return Result.Success(new List<FranchiseOrderDto>()); // empty

        var pathPrefix = root.HierarchyPath;

        // descendant ids as Guid to match Orders.GlobalCustomerId type
        var descendantIds = await db.TenantCustomerProfiles
            .AsNoTracking()
            .Where(p => p.HierarchyPath.StartsWith(pathPrefix))
            .Select(p => p.GlobalCustomerId.ToString())
            .ToListAsync(ct);

        var orders = await db.Orders.AsNoTracking()
            .Where(o => descendantIds.Contains(o.GlobalCustomerId))
            .Select(o => new FranchiseOrderDto(o.Id, o.GlobalCustomerId, o.OrderedAt, o.Total))
            .OrderByDescending(o => o.OrderedAt)
            .ToListAsync(ct);

        return Result.Success(orders);
    }
}
