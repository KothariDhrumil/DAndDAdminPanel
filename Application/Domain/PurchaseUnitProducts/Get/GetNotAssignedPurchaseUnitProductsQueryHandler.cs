using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnitProducts.Get;

public sealed class GetNotAssignedPurchaseUnitProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetNotAssignedPurchaseUnitProductsQuery, List<NotAssignedPurchaseUnitProductResponse>>
{
    public async Task<Result<List<NotAssignedPurchaseUnitProductResponse>>> Handle(GetNotAssignedPurchaseUnitProductsQuery query, CancellationToken ct)
    {
        var assignedProductIds = await db.PurchaseUnitProducts
            .Where(x => x.PurchaseUnitId == query.PurchaseUnitId)
            .Select(x => x.ProductId)
            .ToListAsync(ct);

        var products = await db.Products.AsNoTracking()
            .Where(x => !assignedProductIds.Contains(x.Id))
            .Select(x => new NotAssignedPurchaseUnitProductResponse
            {
                ProductId = x.Id,
                ProductName = x.Name
            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
