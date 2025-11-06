using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Queries;

public sealed record ListCustomersWithTenantsQuery(int PageNumber, int PageSize) 
    : IQuery<PagedResult<List<CustomerWithTenantsDto>>>;

public sealed record CustomerWithTenantsDto(
    Guid GlobalCustomerId,
    string PhoneNumber,
    string? FirstName,
    string? LastName,
    List<CustomerTenantDto> Tenants);

public sealed record CustomerTenantDto(int TenantId, string TenantName);

internal sealed class ListCustomersWithTenantsQueryHandler(AuthPermissionsDbContext db)
    : IQueryHandler<ListCustomersWithTenantsQuery, PagedResult<List<CustomerWithTenantsDto>>>
{
    public async Task<Result<PagedResult<List<CustomerWithTenantsDto>>>> Handle(
        ListCustomersWithTenantsQuery query, CancellationToken ct)
    {
        var page = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var size = query.PageSize <= 0 ? 20 : query.PageSize;

        var baseQuery = db.CustomerAccounts
            .AsNoTracking()
            .OrderBy(c => c.PhoneNumber);

        var total = await baseQuery.CountAsync(ct);

        var items = await baseQuery
            .Skip((page - 1) * size)
            .Take(size)
            .Select(c => new CustomerWithTenantsDto(
                c.GlobalCustomerId,
                c.PhoneNumber,
                c.FirstName,
                c.LastName,
                c.TenantLinks
                    .Select(l => new CustomerTenantDto(
                        l.TenantId,
                        db.Tenants.Where(t => t.TenantId == l.TenantId)
                                  .Select(t => t.TenantFullName)
                                  .FirstOrDefault() ?? string.Empty))
                    .ToList()))
            .ToListAsync(ct);

        var paged = PagedResult<List<CustomerWithTenantsDto>>.Success(items, page, size, total);
        return Result.Success(paged);
    }
}