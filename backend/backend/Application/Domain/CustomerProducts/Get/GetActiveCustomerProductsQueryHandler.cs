using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.CustomerProducts.Get;

public sealed class GetActiveCustomerProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetActiveCustomerProductsQuery, List<ActiveCustomerProductResponse>>
{
    public async Task<Result<List<ActiveCustomerProductResponse>>> Handle(GetActiveCustomerProductsQuery query, CancellationToken ct)
    {
        var products = await db.CustomerProducts.AsNoTracking()
            .Where(x => x.CustomerId == query.CustomerId && x.IsActive && x.Product.IsActive)
            .Include(x => x.Product)
            .Select(x => new ActiveCustomerProductResponse
            {
                ProductId = x.ProductId,
                Name = x.Product.Name,
                SalesRate = x.SalesRate,

            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
