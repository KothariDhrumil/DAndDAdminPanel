using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Get;

public sealed class GetRoutesQueryHandler(IRetailDbContext db) : IQueryHandler<GetRoutesQuery, List<GetRouteResponse>>
{
    public async Task<Result<List<GetRouteResponse>>> Handle(GetRoutesQuery query, CancellationToken ct)
    {
        var routes = await db.Routes.AsNoTracking()
            .Select(x => new GetRouteResponse
            {
                Name = x.Name,
                TenantUserId = x.TenantUserId,
                IsActive = x.IsActive,
                Id = x.Id,
                TenantUser = x.TenantUser.FirstName + " " + x.TenantUser.LastName,
                PriceTier = x.PriceTier.Name,
                PriceTierId = x.PriceTierId
            })
            .ToListAsync(ct);
        return Result.Success(routes);
    }
}
