using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class BulkUpsertPriceTierProductsCommandHandler(IRetailDbContext db) : ICommandHandler<BulkUpsertPriceTierProductsCommand>
{
    public async Task<Result> Handle(BulkUpsertPriceTierProductsCommand command, CancellationToken ct)
    {
        var allTierProduct = await db.PriceTierProducts
            .Where(x => command.Products.Select(p => p.PriceTierId).Contains(x.PriceTierId))
            .ToListAsync(ct);

        foreach (var dto in command.Products)
        {
            var match = allTierProduct.FirstOrDefault(x => x.PriceTierId == dto.PriceTierId && x.ProductId == dto.ProductId);
            if (match == null)
            {
                db.PriceTierProducts.Add(new PriceTierProduct
                {
                    PriceTierId = dto.PriceTierId,
                    ProductId = dto.ProductId,
                    SalesRate = dto.SalesRate
                });
            }
            else
            {
                match.SalesRate = dto.SalesRate;
            }
        }
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
