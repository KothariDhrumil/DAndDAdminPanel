using Application.Abstractions.Messaging;
using Application.SupportTickets.Models;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Queries;

public sealed class GetSupportTicketByIdQuery : IQuery<SupportTicketDetailDto>
{
    public required int Id { get; init; }

    internal sealed class Handler(AuthPermissionsDbContext db) : IQueryHandler<GetSupportTicketByIdQuery, SupportTicketDetailDto>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result<SupportTicketDetailDto>> Handle(GetSupportTicketByIdQuery query, CancellationToken ct)
        {
            var t = await _db.SupportTickets.AsNoTracking()
                .Where(x => x.Id == query.Id)
                .Select(t => new SupportTicketDetailDto
                {
                    Id = t.Id,
                    Message = t.Message,
                    Url = t.Url,
                    Method = t.Method,
                    StatusCode = t.StatusCode,
                    StatusText = t.StatusText,
                    CorrelationId = t.CorrelationId,
                    UserId = t.UserId,
                    TenantId = t.TenantId,
                    TicketStatus = t.TicketStatus,
                    Priority = t.Priority,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    UserAgent = t.UserAgent,
                    Timestamp = t.Timestamp,
                    RequestBody = t.RequestBody,
                    ResponseBody = t.ResponseBody,
                    Headers = t.Headers,
                    Notes = t.Notes,
                    Resolution = t.Resolution
                })
                .FirstOrDefaultAsync(ct);

            return t is null
                ? Result.Failure<SupportTicketDetailDto>(Error.NotFound("SupportTicket.NotFound", $"Ticket {query.Id} not found"))
                : Result.Success(t);
        }
    }
}
