using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Get;

public sealed class GetRouteHandler(IRetailDbContext db) : IQueryHandler<GetRouteQuery, List<GetRouteResponse>>
{
    public async Task<Result<List<GetRouteResponse>>> Handle(GetRouteQuery query, CancellationToken ct)
    {
        var routes = await db.Routes.Where(x => x.IsActive).AsNoTracking()
            .Select(x => new GetRouteResponse
            {
                Name = x.Name,
                Id = x.Id,
            })
            .ToListAsync(ct);
        return Result.Success(routes);
    }
}
