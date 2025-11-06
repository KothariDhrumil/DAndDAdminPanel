using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Routes.Get;

internal sealed class GetPreviousCustomerInRouteQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetPreviousCustomerInRouteQuery, TenantCustomerProfileDto?>
{
    public async Task<Result<TenantCustomerProfileDto?>> Handle(GetPreviousCustomerInRouteQuery query, CancellationToken ct)
    {
        var current = await db.TenantCustomerProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.RouteId == query.RouteId && c.TenantUserId == query.CustomerId, ct);
        if (current == null)
            return Result.Success<TenantCustomerProfileDto?>(null);
        var prev = await db.TenantCustomerProfiles
            .AsNoTracking()
            .Where(c => c.RouteId == query.RouteId && c.SequenceNo < current.SequenceNo)
            .OrderByDescending(c => c.SequenceNo)
            .Select(c => new TenantCustomerProfileDto
            {
                TenantUserId = c.TenantUserId,
                FirstName = c.FirstName,
                LastName = c.LastName
            })
            .FirstOrDefaultAsync(ct);
        return Result.Success(prev);
    }
}
