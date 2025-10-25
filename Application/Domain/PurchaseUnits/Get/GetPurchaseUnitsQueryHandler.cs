using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.Get;

public sealed class GetPurchaseUnitsQueryHandler(IRetailDbContext db) : IQueryHandler<GetPurchaseUnitsQuery, List<GetPurchaseUnitResponse>>
{
    public async Task<Result<List<GetPurchaseUnitResponse>>> Handle(GetPurchaseUnitsQuery query, CancellationToken ct)
    {
        var units = await db.PurchaseUnits.AsNoTracking()
            .Select(x => new GetPurchaseUnitResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsInternal = x.IsInternal,
                Address = x.Address,
                IsTaxable = x.IsTaxable,
                TenantUserId = x.TenantUserId,
                TenantUser = x.TenantUser != null ? $"{x.TenantUser.FirstName} {x.TenantUser.LastName}" : string.Empty,
                IsActive = x.IsActive
            })
            .ToListAsync(ct);
        return Result.Success(units);
    }
}
