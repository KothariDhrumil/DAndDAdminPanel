using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.GetById;

public sealed class GetRouteByIdQueryHandler(IRetailDbContext db) : IQueryHandler<GetRouteByIdQuery, RouteResponse>
{
    public async Task<Result<RouteResponse>> Handle(GetRouteByIdQuery query, CancellationToken ct)
    {
        var route = await db.Routes.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new RouteResponse
            {
                Name = x.Name,
                TenantUserId = x.TenantUserId,
                IsActive = x.IsActive,
                Id = x.Id
            })
            .SingleOrDefaultAsync(ct);
        return route == null
            ? Result.Failure<RouteResponse>(Error.NotFound("RouteNotFound", "Route not found."))
            : Result.Success(route);
    }
}
