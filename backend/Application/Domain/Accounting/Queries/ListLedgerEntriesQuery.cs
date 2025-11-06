using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Accounting.Queries;

public sealed record ListLedgerEntriesQuery(
    int TenantId,
    int PageNumber,
    int PageSize,
    DateTime? From,
    DateTime? To,
    Guid TenantUserId
) : IQuery<PagedResult<List<LedgerEntryDto>>>;

public sealed record LedgerEntryDto(long Id, DateTime EntryDate, decimal Amount, string Description, string? ReferenceId, Guid TenantUserId);

internal sealed class Handler(ITenantRetailDbContextFactory factory) : IQueryHandler<ListLedgerEntriesQuery, PagedResult<List<LedgerEntryDto>>>
{
    public async Task<Result<PagedResult<List<LedgerEntryDto>>>> Handle(ListLedgerEntriesQuery query, CancellationToken ct)
    {
        int page = query.PageNumber <= 0 ? 1 : query.PageNumber;
        int size = query.PageSize <= 0 ? 20 : query.PageSize;

        var db = await factory.CreateAsync(query.TenantId, ct);
        var q = db.LedgerEntries.AsNoTracking();

        if (query.From.HasValue) q = q.Where(x => x.EntryDate >= query.From.Value);
        if (query.To.HasValue) q = q.Where(x => x.EntryDate <= query.To.Value);

        q = q.Where(x => x.TenantUserId == query.TenantUserId);

        int total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(x => x.EntryDate)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(x => new LedgerEntryDto(x.Id, x.EntryDate, x.Amount, x.Description, x.ReferenceId, x.TenantUserId))
            .ToListAsync(ct);

        var result = PagedResult<List<LedgerEntryDto>>.Success(items, page, size, total);
        return Result.Success(result);
    }
}
