using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Queries;

public sealed record ListCustomersByTenantQuery(int TenantId, int PageNumber, int PageSize)
    : IQuery<PagedResult<List<TenantCustomerDto>>>;

public sealed record TenantCustomerDto(
    Guid GlobalCustomerId,
    string PhoneNumber,
    string? FirstName,
    string? LastName);

internal sealed class ListCustomersByTenantQueryHandler(AuthPermissionsDbContext db)
    : IQueryHandler<ListCustomersByTenantQuery, PagedResult<List<TenantCustomerDto>>>
{
    public async Task<Result<PagedResult<List<TenantCustomerDto>>>> Handle(
        ListCustomersByTenantQuery query, CancellationToken ct)
    {
        var page = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var size = query.PageSize <= 0 ? 20 : query.PageSize;

        var baseQuery = db.CustomerTenantLinks
            .AsNoTracking()
            .Where(l => l.TenantId == query.TenantId)
            .Join(db.CustomerAccounts,
                l => l.GlobalCustomerId,
                c => c.GlobalCustomerId,
                (l, c) => c)
            .OrderBy(c => c.PhoneNumber);

        var total = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .Skip((page - 1) * size)
            .Take(size)
            .Select(c => new TenantCustomerDto(
                c.GlobalCustomerId,
                c.PhoneNumber,
                c.FirstName,
                c.LastName))
            .ToListAsync(ct);

        var paged = PagedResult<List<TenantCustomerDto>>.Success(items, page, size, total);
        return Result.Success(paged);
    }
}