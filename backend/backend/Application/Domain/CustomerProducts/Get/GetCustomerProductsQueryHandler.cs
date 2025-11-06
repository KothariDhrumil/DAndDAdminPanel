using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.CustomerProducts.Get;

public sealed class GetCustomerProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetCustomerProductsQuery, List<CustomerProductResponse>>
{
    public async Task<Result<List<CustomerProductResponse>>> Handle(GetCustomerProductsQuery query, CancellationToken ct)
    {
        var products = await db.CustomerProducts.AsNoTracking()
            .Where(x => x.CustomerId == query.CustomerId)
            .Include(x => x.Product)
            .Select(x => new CustomerProductResponse
            {
                ProductId = x.ProductId,
                Name = x.Product.Name,
                ThumbnailPath = x.Product.ThumbnailPath,
                SalesRate = x.SalesRate,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
