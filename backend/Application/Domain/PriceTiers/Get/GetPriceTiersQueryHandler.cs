using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Get;

public sealed class GetPriceTiersQueryHandler(IRetailDbContext db) : IQueryHandler<GetPriceTiersQuery, List<PriceTierResponse>>
{
    public async Task<Result<List<PriceTierResponse>>> Handle(GetPriceTiersQuery query, CancellationToken ct)
    {
        var tiers = await db.PriceTiers.AsNoTracking()
            .Select(x => new PriceTierResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
        return Result.Success(tiers);
    }
}
