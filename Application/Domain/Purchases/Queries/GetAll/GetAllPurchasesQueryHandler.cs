using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Queries.GetAll;

public sealed class GetAllPurchasesQueryHandler(IRetailDbContext db) 
    : IQueryHandler<GetAllPurchasesQuery, List<PurchaseResponse>>
{
    public async Task<Result<List<PurchaseResponse>>> Handle(GetAllPurchasesQuery query, CancellationToken ct)
    {
        var purchasesQuery = db.Purchases
            .Include(p => p.Route)
            .Include(p => p.PurchaseUnit)
            .Include(p => p.PickupSalesman)
            .AsQueryable();

        // Apply filters
        if (query.StartDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate >= query.StartDate.Value);

        if (query.EndDate.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseDate <= query.EndDate.Value);

        if (query.RouteId.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.RouteId == query.RouteId.Value);

        if (query.PurchaseUnitId.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.PurchaseUnitId == query.PurchaseUnitId.Value);

        if (query.IsConfirmed.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.IsConfirmed == query.IsConfirmed.Value);

        if (query.IsPreOrder.HasValue)
            purchasesQuery = purchasesQuery.Where(p => p.IsPreOrder == query.IsPreOrder.Value);

        var purchases = await purchasesQuery
            .OrderByDescending(p => p.PurchaseDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new PurchaseResponse
            {
                Id = p.Id,
                RouteId = p.RouteId,
                RouteName = p.Route != null ? p.Route.Name : null,
                PurchaseUnitId = p.PurchaseUnitId,
                PurchaseUnitName = p.PurchaseUnit.Name,
                PurchaseDate = p.PurchaseDate,
                OrderPickupDate = p.OrderPickupDate,
                Amount = p.Amount,
                Discount = p.Discount,
                Tax = p.Tax,
                AdditionalTax = p.AdditionalTax,
                GrandTotal = p.GrandTotal,
                Remarks = p.Remarks,
                IsConfirmed = p.IsConfirmed,
                IsPreOrder = p.IsPreOrder,
                PickupSalesmanId = p.PickupSalesmanId,
                PickupSalesmanName = p.PickupSalesman != null ? $"{p.PickupSalesman.FirstName} {p.PickupSalesman.LastName}" : null,
                Type = p.Type
            })
            .ToListAsync(ct);

        return Result.Success(purchases);
    }
}
