using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.GetById;

public sealed class GetPurchaseUnitByIdQueryHandler(IRetailDbContext db) : IQueryHandler<GetPurchaseUnitByIdQuery, PurchaseUnitResponse>
{
    public async Task<Result<PurchaseUnitResponse>> Handle(GetPurchaseUnitByIdQuery query, CancellationToken ct)
    {
        var unit = await db.PurchaseUnits.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new PurchaseUnitResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsInternal = x.IsInternal,
                Address = x.Address,
                IsTaxable = x.IsTaxable,
                TenantUserId = x.TenantUserId
            })
            .SingleOrDefaultAsync(ct);
        return unit == null
            ? Result.Failure<PurchaseUnitResponse>(Error.NotFound("PurchaseUnitNotFound", "Purchase unit not found."))
            : Result.Success(unit);
    }
}
