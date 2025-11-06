using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class BulkUpsertRoutePriceTiersCommandHandler(IRetailDbContext db) : ICommandHandler<BulkUpsertRoutePriceTiersCommand>
{
    public async Task<Result> Handle(BulkUpsertRoutePriceTiersCommand command, CancellationToken ct)
    {
        //var allRouteTiers = await db.RoutePriceTiers
        //    .Where(x => command.RouteTiers.Select(r => r.RouteId).Contains(x.RouteId))
        //    .ToListAsync(ct);

        //foreach (var dto in command.RouteTiers)
        //{
        //    var match = allRouteTiers.FirstOrDefault(x => x.RouteId == dto.RouteId);
        //    if (match == null)
        //    {
        //        db.RoutePriceTiers.Add(new RoutePriceTier
        //        {
        //            RouteId = dto.RouteId,
        //            PriceTierId = dto.PriceTierId
        //        });
        //    }
        //    else
        //    {
        //        match.PriceTierId = dto.PriceTierId;
        //    }
        //}
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
