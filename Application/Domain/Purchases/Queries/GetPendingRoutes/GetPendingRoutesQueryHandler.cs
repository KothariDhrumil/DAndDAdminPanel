using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Queries.GetPendingRoutes;

public sealed class GetPendingRoutesQueryHandler(IRetailDbContext db) 
    : IQueryHandler<GetPendingRoutesQuery, List<PendingRouteResponse>>
{
    public async Task<Result<List<PendingRouteResponse>>> Handle(GetPendingRoutesQuery query, CancellationToken ct)
    {
        var purchasesQuery = db.Purchases
            .Include(p => p.Route)
            .Where(p => p.RouteId.HasValue && p.IsConfirmed == false);

        // Apply filters
        if (query.StartDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate >= query.StartDate.Value);

        if (query.EndDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate <= query.EndDate.Value);

        var pendingRoutes = await purchasesQuery
            .GroupBy(p => new { p.RouteId, p.Route!.Name })
            .Select(g => new PendingRouteResponse
            {
                RouteId = g.Key.RouteId!.Value,
                RouteName = g.Key.Name,
                PendingPurchaseCount = g.Count(),
                TotalPendingAmount = g.Sum(p => p.GrandTotal)
            })
            .OrderByDescending(r => r.PendingPurchaseCount)
            .ToListAsync(ct);

        return Result.Success(pendingRoutes);
    }
}
