using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.GetById;

public sealed class GetProductByIdQueryHandler(IRetailDbContext db) : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await db.Products.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                Description = x.Description,
                HSNCode = x.HSNCode,
                IGST = x.IGST,
                CGST = x.CGST,
                BasePrice = x.BasePrice,
                Order = x.Order,
                HindiContent = x.HindiContent                
            })
            .SingleOrDefaultAsync(ct);
        return product == null
            ? Result.Failure<ProductResponse>(Error.NotFound("ProductNotFound", "Product not found."))
            : Result.Success(product);
    }
}
