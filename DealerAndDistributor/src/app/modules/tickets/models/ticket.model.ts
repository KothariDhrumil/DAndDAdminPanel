export type TicketId = string;

export interface Ticket {
    id: TicketId;
    message: string;
    url?: string;
    ticketStatus: TicketStatus;
    priority: TicketPriority;
    createdAt: string;
    updatedAt?: string;
    createdBy?: string;
    assignedTo?: string;
    correlationId?: string;
    tenantId?: number;
}

export interface TicketDetails extends Ticket {
    // HTTP context and diagnostics returned by backend
    method?: string;                 // e.g., 'GET' | 'POST'
    statusCode?: number;             // e.g., 500
    statusText?: string;             // e.g., 'Unknown Error'
    userAgent?: string;              // browser UA string
    timestamp?: string;              // ISO string when error occurred

    // Raw payloads as JSON strings (may be null or empty)
    requestBody?: string | null;
    responseBody?: string | null;
    headers?: string | null;

    // Workflow fields
    notes?: string | null;
    resolution?: string | null;

    // Ownership/identity
    userId?: string | null;
}


export enum TicketStatus {
    Open = 0,
    InProgress = 1,
    Resolved = 2,
    Closed = 3,
}

export enum TicketPriority {
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3,
}

export interface TicketComment {
    id: string;
    ticketId: TicketId;
    message: string;
    createdAt: string;
    createdBy?: string;
}

export interface PagedQuery {
    pageNumber: number;
    pageSize: number;
    search?: string;
    sortBy?: string;
    sortDir?: 'asc' | 'desc';
    filters?: Record<string, string | number | boolean>;
}

export interface PagedResult<T> {
    data: T[];
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
}

// Payload for updating a ticket via PUT /api/support-tickets/{id}
export interface UpdateTicketPayload {
    id: TicketId;
    notes: string;
    ticketStatus: TicketStatus;
    priority: TicketPriority;
    resolution: string;
}
