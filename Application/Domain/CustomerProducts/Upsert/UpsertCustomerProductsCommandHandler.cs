using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.CustomerProducts.Upsert;

public sealed class UpsertCustomerProductsCommandHandler(IRetailDbContext db) : ICommandHandler<UpsertCustomerProductsCommand>
{
    public async Task<Result> Handle(UpsertCustomerProductsCommand command, CancellationToken ct)
    {
        var existing = await db.CustomerProducts
            .Where(x => x.CustomerId == command.CustomerId)
            .ToListAsync(ct);

        foreach (var dto in command.Products)
        {
            var match = existing.FirstOrDefault(x => x.ProductId == dto.ProductId);
            if (match == null)
            {
                db.CustomerProducts.Add(new CustomerProduct
                {
                    CustomerId = command.CustomerId,
                    ProductId = dto.ProductId,
                    SalesRate = dto.SalesRate,
                    IsActive = dto.IsActive
                });
            }
            else
            {
                match.SalesRate = dto.SalesRate;
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
