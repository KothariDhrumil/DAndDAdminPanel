using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnitProducts.Upsert;

public sealed class UpsertPurchaseUnitProductsCommandHandler(IRetailDbContext db) : ICommandHandler<UpsertPurchaseUnitProductsCommand>
{
    public async Task<Result> Handle(UpsertPurchaseUnitProductsCommand command, CancellationToken ct)
    {
        var existing = await db.PurchaseUnitProducts
            .Where(x => x.PurchaseUnitId == command.PurchaseUnitId)
            .ToListAsync(ct);

        foreach (var dto in command.Products)
        {
            var match = existing.FirstOrDefault(x => x.ProductId == dto.ProductId);
            if (match == null)
            {
                db.PurchaseUnitProducts.Add(new PurchaseUnitProduct
                {
                    PurchaseUnitId = command.PurchaseUnitId,
                    ProductId = dto.ProductId,
                    PurchaseRate = dto.PurchaseRate,
                    Tax = dto.Tax,
                    IsActive = dto.IsActive
                });
            }
            else
            {
                match.PurchaseRate = dto.PurchaseRate;
                match.Tax = dto.Tax;
                match.IsActive = dto.IsActive;
            }
        }

        // Deactivate products not present in the upsert list
        var upsertIds = command.Products.Select(x => x.ProductId).ToHashSet();
        foreach (var prod in existing)
        {
            if (!upsertIds.Contains(prod.ProductId))
            {
                prod.IsActive = false;
            }
        }

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
