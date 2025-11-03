using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Queries.GetById;

public sealed class GetPurchaseByIdQueryHandler(IRetailDbContext db) 
    : IQueryHandler<GetPurchaseByIdQuery, PurchaseDetailResponse>
{
    public async Task<Result<PurchaseDetailResponse>> Handle(GetPurchaseByIdQuery query, CancellationToken ct)
    {
        var purchase = await db.Purchases
            .Include(p => p.Route)
            .Include(p => p.PurchaseUnit)
            .Include(p => p.PickupSalesman)
            .Include(p => p.PurchaseDetails)
                .ThenInclude(pd => pd.Product)
            .Where(p => p.Id == query.PurchaseId)
            .Select(p => new PurchaseDetailResponse
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
                Type = p.Type,
                PurchaseDetails = p.PurchaseDetails.Select(pd => new PurchaseDetailItemResponse
                {
                    Id = pd.Id,
                    ProductId = pd.ProductId,
                    ProductName = pd.Product.Name,
                    Qty = pd.Qty,
                    Rate = pd.Rate,
                    Tax = pd.Tax,
                    Amount = pd.Amount
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (purchase == null)
            return Result.Failure<PurchaseDetailResponse>(
                Error.NotFound("Purchase.NotFound", "Purchase not found"));

        return Result.Success(purchase);
    }
}
