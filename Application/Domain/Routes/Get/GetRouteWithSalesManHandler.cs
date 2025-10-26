using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Get;

public sealed class GetRouteWithSalesManHandler(IRetailDbContext db) : IQueryHandler<GetRouteWithSalesManQuery, List<GetRouteWithSalesManResponse>>
{
    public async Task<Result<List<GetRouteWithSalesManResponse>>> Handle(GetRouteWithSalesManQuery query, CancellationToken ct)
    {
        var routes = await db.Routes.AsNoTracking()
            .Select(x => new GetRouteWithSalesManResponse
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
