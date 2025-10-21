using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Pricing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.CustomerProducts.Get;

public sealed class GetNotAssignedProductsQueryHandler(IRetailDbContext db, IPriceTierService priceTierService) : IQueryHandler<GetNotAssignedProductsQuery, List<NotAssignedProductResponse>>
{
    public async Task<Result<List<NotAssignedProductResponse>>> Handle(GetNotAssignedProductsQuery query, CancellationToken ct)
    {
        var assignedProductIds = await db.CustomerProducts
            .Where(x => x.CustomerId == query.CustomerId)
            .Select(x => x.ProductId)
            .ToListAsync(ct);

        var products = await db.Products.AsNoTracking()
            .Where(x => !assignedProductIds.Contains(x.Id))
            .ToListAsync(ct);

        var result = new List<NotAssignedProductResponse>();
        foreach (var product in products)
        {
            var salesRate = await priceTierService.GetSalesRateAsync(query.CustomerId, product.Id, ct);
            result.Add(new NotAssignedProductResponse
            {
                ProductId = product.Id,
                Name = product.Name,
                ThumbnailPath = product.ThumbnailPath,
                SalesRate = salesRate ?? product.BasePrice
            });
        }
        return Result.Success(result);
    }
}
