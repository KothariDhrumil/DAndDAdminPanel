using System.Net;
using Application.Abstractions.Messaging;
using Application.SupportTickets.Models;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Queries;

public sealed class SearchSupportTicketsQuery : IQuery<PagedResult<List<SupportTicketListItemDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; init; }
    public string? SortBy { get; init; }
    public string? SortDir { get; init; }
    public TicketStatusDTO? State { get; init; }
    public TicketPriorityDTO? Priority { get; init; }
    public int? TenantId { get; init; }
    public string? UserId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public HttpStatusCode? HttpStatus { get; init; }
    public string? Method { get; init; }
    public string? CorrelationId { get; init; }

    internal sealed class Handler(AuthPermissionsDbContext db) : IQueryHandler<SearchSupportTicketsQuery, PagedResult<List<SupportTicketListItemDto>>>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result<PagedResult<List<SupportTicketListItemDto>>>> Handle(SearchSupportTicketsQuery query, CancellationToken ct)
        {
            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            IQueryable<SupportTicket> q = _db.SupportTickets.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                string term = query.Search.Trim();
                q = q.Where(t =>
                    t.Message.Contains(term) ||
                    t.Url.Contains(term) ||
                    t.StatusText.Contains(term) ||
                    t.CorrelationId.Contains(term) ||
                    t.UserAgent.Contains(term) ||
                    (t.Notes != null && t.Notes.Contains(term)));
            }

            //if (query.State.HasValue) q = q.Where(t => t.TicketStatus ==(int) query.State);
            //if (query.Priority.HasValue) q = q.Where(t => t.Priority == query.Priority);
            if (query.TenantId.HasValue) q = q.Where(t => t.TenantId == query.TenantId);
            if (!string.IsNullOrWhiteSpace(query.UserId)) q = q.Where(t => t.UserId == query.UserId);
            if (query.From.HasValue) q = q.Where(t => t.CreatedAt >= query.From);
            if (query.To.HasValue) q = q.Where(t => t.CreatedAt <= query.To);
            if (query.HttpStatus.HasValue) q = q.Where(t => t.StatusCode == query.HttpStatus);
            if (!string.IsNullOrWhiteSpace(query.Method)) q = q.Where(t => t.Method == query.Method);
            if (!string.IsNullOrWhiteSpace(query.CorrelationId)) q = q.Where(t => t.CorrelationId == query.CorrelationId);

            q = (query.SortBy?.ToLowerInvariant(), query.SortDir?.ToLowerInvariant()) switch
            {
                ("updatedat", "asc") => q.OrderBy(t => t.UpdatedAt),
                ("updatedat", _) => q.OrderByDescending(t => t.UpdatedAt),
                ("priority", "asc") => q.OrderBy(t => t.Priority),
                ("priority", _) => q.OrderByDescending(t => t.Priority),
                ("state", "asc") => q.OrderBy(t => t.TicketStatus),
                ("state", _) => q.OrderByDescending(t => t.TicketStatus),
                ("status", "asc") => q.OrderBy(t => t.StatusCode),
                ("status", _) => q.OrderByDescending(t => t.StatusCode),
                ("createdat", "asc") => q.OrderBy(t => t.CreatedAt),
                _ => q.OrderByDescending(t => t.CreatedAt)
            };

            int total = await q.CountAsync(ct);

            var items = await q
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new SupportTicketListItemDto
                {
                    Id = t.Id,
                    Message = t.Message,
                    Url = t.Url,
                    Method = t.Method,
                    StatusCode = t.StatusCode,
                    StatusText = t.StatusText,
                    UserId = t.UserId,
                    TenantId = t.TenantId,
                    TicketStatus = (TicketStatusDTO)t.TicketStatus,
                    Priority = (TicketPriorityDTO)t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync(ct);

            var page = PagedResult<List<SupportTicketListItemDto>>.Success(items, pageNumber, pageSize, total);
            return Result.Success(page);
        }
    }
}
