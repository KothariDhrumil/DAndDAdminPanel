using System.Net;

namespace Application.SupportTickets.Models;

public class SupportTicketListItemDto
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public int? TenantId { get; set; }
    public TicketStatusDTO TicketStatus { get; set; }
    public TicketPriorityDTO Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SupportTicketDetailDto : SupportTicketListItemDto
{
    public string CorrelationId { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string RequestBody { get; set; } = string.Empty;
    public string ResponseBody { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string Resolution { get; set; } = string.Empty;
}

public class SupportTicketUpdateRequest
{
    public string? Notes { get; set; }
    public TicketStatusDTO? State { get; set; }
    public TicketPriorityDTO? Priority { get; set; }
    public string? Resolution { get; set; }
}

public class SupportTicketQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
    public string? SortBy { get; set; } // createdAt, updatedAt, priority, state, status
    public string? SortDir { get; set; } // asc or desc

    public TicketStatusDTO? State { get; set; }
    public TicketPriorityDTO? Priority { get; set; }
    public int? TenantId { get; set; }
    public string? UserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public HttpStatusCode? HttpStatus { get; set; }
    public string? Method { get; set; }
    public string? CorrelationId { get; set; }
}


public enum TicketStatusDTO
{
    Open,
    InProgress,
    Resolved,
    Closed
}

public enum TicketPriorityDTO
{
    Low,
    Medium,
    High,
    Urgent
}


