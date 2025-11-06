using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Queries;

public sealed record ListTenantCustomerProfilesQuery(
    int TenantId,
    int PageNumber,
    int PageSize,
    string? Search
) : IQuery<PagedResult<List<TenantCustomerProfileDto>>>;

public sealed record TenantCustomerProfileDto(
    Guid TenantCustomerId,
    Guid GlobalCustomerId,
    int TenantId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber);

internal sealed class ListTenantCustomerProfilesQueryHandler(
    ITenantRetailDbContextFactory tenantRetailDbContextFactory
) : IQueryHandler<ListTenantCustomerProfilesQuery, PagedResult<List<TenantCustomerProfileDto>>>
{
    public async Task<Result<PagedResult<List<TenantCustomerProfileDto>>>> Handle(
        ListTenantCustomerProfilesQuery query,
        CancellationToken cancellationToken)
    {
        var page = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var size = query.PageSize <= 0 ? 20 : query.PageSize;

        // Open tenant-specific RetailDbContext (DataKey filter applied automatically)
        var db = await tenantRetailDbContextFactory.CreateAsync(query.TenantId, cancellationToken);

        var profiles = db.TenantCustomerProfiles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim();
            profiles = profiles.Where(p =>
                (p.FirstName != null && p.FirstName.Contains(term))); // || (p.Preferences != null && p.Preferences.Contains(term))
        }

        var total = await profiles.CountAsync(cancellationToken);

        var pageItems = await profiles
            .OrderBy(p => p.FirstName)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(p => new TenantCustomerProfileDto(
                p.TenantUserId,
                p.GlobalCustomerId,
                p.TenantId,
                p.FirstName,
                p.LastName,
                p.PhoneNumber
                ))
            .ToListAsync(cancellationToken);

        var paged = PagedResult<List<TenantCustomerProfileDto>>.Success(pageItems, page, size, total);
        return Result.Success(paged);
    }
}