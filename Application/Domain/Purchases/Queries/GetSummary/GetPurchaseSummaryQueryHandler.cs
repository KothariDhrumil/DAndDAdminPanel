using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Queries.GetSummary;

public sealed class GetPurchaseSummaryQueryHandler(IRetailDbContext db) 
    : IQueryHandler<GetPurchaseSummaryQuery, PurchaseSummaryResponse>
{
    public async Task<Result<PurchaseSummaryResponse>> Handle(GetPurchaseSummaryQuery query, CancellationToken ct)
    {
        var purchasesQuery = db.Purchases.AsQueryable();

        // Apply filters
        if (query.StartDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate >= query.StartDate.Value);

        if (query.EndDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate <= query.EndDate.Value);

        if (query.RouteId.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.RouteId == query.RouteId.Value);

        if (query.PurchaseUnitId.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseUnitId == query.PurchaseUnitId.Value);

        var summary = new PurchaseSummaryResponse
        {
            TotalPurchases = await purchasesQuery.CountAsync(ct),
            ConfirmedPurchases = await purchasesQuery.CountAsync(p => p.IsConfirmed, ct),
            PreOrders = await purchasesQuery.CountAsync(p => p.IsPreOrder, ct),
            TotalAmount = await purchasesQuery.SumAsync(p => (decimal?)p.GrandTotal, ct) ?? 0m,
            ConfirmedAmount = await purchasesQuery
                .Where(p => p.IsConfirmed)
                .SumAsync(p => (decimal?)p.GrandTotal, ct) ?? 0m,
            PreOrderAmount = await purchasesQuery
                .Where(p => p.IsPreOrder)
                .SumAsync(p => (decimal?)p.GrandTotal, ct) ?? 0m
        };

        return Result.Success(summary);
    }
}
