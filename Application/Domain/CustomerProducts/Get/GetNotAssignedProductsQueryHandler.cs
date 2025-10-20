using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.CustomerProducts.Get;

public sealed class GetNotAssignedProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetNotAssignedProductsQuery, List<NotAssignedProductResponse>>
{
    public async Task<Result<List<NotAssignedProductResponse>>> Handle(GetNotAssignedProductsQuery query, CancellationToken ct)
    {
        var assignedProductIds = await db.CustomerProducts
            .Where(x => x.CustomerId == query.CustomerId)
            .Select(x => x.ProductId)
            .ToListAsync(ct);

        var products = await db.Products.AsNoTracking()
            .Where(x => !assignedProductIds.Contains(x.Id))
            .Select(x => new NotAssignedProductResponse
            {
                ProductId = x.Id,
                Name = x.Name,
                ThumbnailPath = x.ThumbnailPath
            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
