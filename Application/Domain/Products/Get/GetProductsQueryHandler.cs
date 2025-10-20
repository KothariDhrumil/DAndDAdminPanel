using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.Get;

public sealed class GetProductsQueryHandler(IRetailDbContext db) : IQueryHandler<GetProductsQuery, List<GetProductsResponse>>
{
    public async Task<Result<List<GetProductsResponse>>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = await db.Products.AsNoTracking()
            .Select(x => new GetProductsResponse
            {
                Id = x.Id,
                Name = x.Name,
                ThumbnailPath = x.ThumbnailPath,
                Description = x.Description,
                HSNCode = x.HSNCode,
                IGST = x.IGST,
                CGST = x.CGST,
                BasePrice = x.BasePrice,
                Order = x.Order,
                HindiContent = x.HindiContent,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
        return Result.Success(products);
    }
}
