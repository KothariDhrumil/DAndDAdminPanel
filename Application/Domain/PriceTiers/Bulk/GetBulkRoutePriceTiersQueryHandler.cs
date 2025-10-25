using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class GetBulkRoutePriceTiersQueryHandler(IRetailDbContext db) : IQueryHandler<GetBulkRoutePriceTiersQuery, List<RoutePriceTierBulkResponse>>
{
    public async Task<Result<List<RoutePriceTierBulkResponse>>> Handle(GetBulkRoutePriceTiersQuery query, CancellationToken ct)
    {
        //var items = await db.RoutePriceTiers
        //    .Include(x => x.Route)
        //    .Include(x => x.PriceTier)
        //    .Select(x => new RoutePriceTierBulkResponse
        //    {
        //        RouteId = x.RouteId,
        //        RouteName = x.Route.Name,
        //        PriceTierId = x.PriceTierId,
        //        PriceTierName = x.PriceTier.Name
        //    })
        //    .ToListAsync(ct);
        return Result.Success(new List<RoutePriceTierBulkResponse>());
    }
}
