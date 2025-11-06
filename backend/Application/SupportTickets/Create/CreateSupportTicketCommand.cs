using System.Net;
using System.Text.Json;
using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Create;

public sealed class CreateSupportTicketCommand : ICommand<int>
{
    public string Message { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public HttpStatusCode Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public JsonElement? RequestBody { get; set; }
    public JsonElement? ResponseBody { get; set; }
    public JsonElement? Headers { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public int? TenantId { get; set; }
    public string? Notes { get; set; }
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;

    internal sealed class Handler(AuthPermissionsDbContext db) : ICommandHandler<CreateSupportTicketCommand, int>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result<int>> Handle(CreateSupportTicketCommand command, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            static string ToStringOrJson(JsonElement? el)
            {
                if (el is null) return string.Empty;
                if (!el.HasValue) return string.Empty;
                var value = el.Value;
                return value.ValueKind switch
                {
                    JsonValueKind.String => value.GetString() ?? string.Empty,
                    JsonValueKind.Undefined => string.Empty,
                    JsonValueKind.Null => string.Empty,
                    _ => JsonSerializer.Serialize(value)
                };
            }

            var ticket = new SupportTicket
            {
                Message = command.Message ?? string.Empty,
                Url = command.Url ?? string.Empty,
                Method = command.Method ?? string.Empty,
                StatusCode = command.Status,
                StatusText = command.StatusText ?? string.Empty,
                UserAgent = command.UserAgent ?? string.Empty,
                Timestamp = (command.Timestamp ?? DateTimeOffset.UtcNow).UtcDateTime,
                RequestBody = ToStringOrJson(command.RequestBody),
                ResponseBody = ToStringOrJson(command.ResponseBody),
                Headers = ToStringOrJson(command.Headers),
                CorrelationId = command.CorrelationId ?? string.Empty,
                UserId = command.UserId,
                TenantId = command.TenantId,
                Notes = command.Notes,
                TicketStatus = TicketStatus.Open,
                CreatedAt = now,
                UpdatedAt = now,
                Priority = command.Priority,
                Resolution = string.Empty
            };

            _db.SupportTickets.Add(ticket);
            await _db.SaveChangesAsync(ct);

            return Result.Success(ticket.Id);
        }
    }
}

public sealed class CreateSupportTicketCommandValidator : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty().MaximumLength(4096);
        RuleFor(x => x.Url).MaximumLength(2048);
        RuleFor(x => x.Method).MaximumLength(16);
        RuleFor(x => x.StatusText).MaximumLength(256);
        RuleFor(x => x.UserAgent).MaximumLength(1024);
        RuleFor(x => x.CorrelationId).MaximumLength(128);
        RuleFor(x => x.Priority).IsInEnum();
    }
}
