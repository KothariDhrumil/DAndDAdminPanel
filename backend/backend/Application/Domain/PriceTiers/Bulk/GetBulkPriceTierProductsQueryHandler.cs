using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class GetBulkPriceTierProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetBulkPriceTierProductsQuery, List<PriceTierProductBulkResponse>>
{
    public async Task<Result<List<PriceTierProductBulkResponse>>> Handle(GetBulkPriceTierProductsQuery query, CancellationToken ct)
    {
        var tiers = await db.PriceTiers.Where(x => x.IsActive).ToListAsync(ct);
        var products = await db.Products.ToListAsync(ct);
        var tierProducts = await db.PriceTierProducts.ToListAsync(ct);

        var result = (
            from tier in tiers
            from product in products
            let ptp = tierProducts.FirstOrDefault(x => x.PriceTierId == tier.Id && x.ProductId == product.Id)
            select new PriceTierProductBulkResponse
            {
                PriceTierId = tier.Id,
                PriceTierName = tier.Name,
                ProductId = product.Id,
                ProductName = product.Name,
                SalesRate = ptp?.SalesRate ?? product.BasePrice
            }
        ).ToList();

        return Result.Success(result);
    }
}
