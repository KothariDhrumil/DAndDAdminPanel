using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnitProducts.Get;

public sealed class GetPurchaseUnitProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetPurchaseUnitProductsQuery, List<PurchaseUnitProductResponse>>
{
    public async Task<Result<List<PurchaseUnitProductResponse>>> Handle(GetPurchaseUnitProductsQuery query, CancellationToken ct)
    {
        var products = await db.PurchaseUnitProducts.AsNoTracking()
            .Where(x => x.PurchaseUnitId == query.PurchaseUnitId)
            .Include(x => x.Product)
            .Select(x => new PurchaseUnitProductResponse
            {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                PurchaseRate = x.PurchaseRate,
                Tax = x.Tax,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
