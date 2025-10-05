import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, PaginatedApiResponse } from '../../../core/models/interface/ApiResponse';
import { Ticket, TicketComment, TicketId, PagedQuery, TicketStatus } from '../models/ticket.model';
import { 
  SUPPORT_TICKETS_API,
  SUPPORT_TICKET_BY_ID_API,
  SUPPORT_TICKET_COMMENTS_API,
  SUPPORT_TICKET_STATUS_API,
  SUPPORT_TICKET_ASSIGN_API,
} from '../../../core/helpers/routes/api-endpoints';

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private http = inject(HttpClient);

  create(ticket: Partial<Ticket>): Observable<ApiResponse<Ticket>> {
    return this.http.post<ApiResponse<Ticket>>(SUPPORT_TICKETS_API, ticket);
  }

  list(query: PagedQuery): Observable<PaginatedApiResponse<Ticket[]>> {
    let params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);
    if (query.search) params = params.set('search', query.search);
    if (query.sortBy) params = params.set('sortBy', query.sortBy);
    if (query.sortDir) params = params.set('sortDir', query.sortDir);
    if (query.filters) {
      Object.entries(query.filters).forEach(([k, v]) => {
        params = params.set(k, String(v));
      });
    }
    return this.http.get<PaginatedApiResponse<Ticket[]>>(SUPPORT_TICKETS_API, { params });
  }

  getById(id: TicketId): Observable<ApiResponse<Ticket>> {
    return this.http.get<ApiResponse<Ticket>>(SUPPORT_TICKET_BY_ID_API(id));
  }

  update(id: TicketId, patch: Partial<Ticket>): Observable<ApiResponse<Ticket>> {
    return this.http.put<ApiResponse<Ticket>>(SUPPORT_TICKET_BY_ID_API(id), patch);
  }

  updateStatus(id: TicketId, status: TicketStatus): Observable<ApiResponse<Ticket>> {
    return this.http.patch<ApiResponse<Ticket>>(SUPPORT_TICKET_STATUS_API(id), { status });
  }

  assign(id: TicketId, userId: string): Observable<ApiResponse<Ticket>> {
    return this.http.post<ApiResponse<Ticket>>(SUPPORT_TICKET_ASSIGN_API(id), { userId });
  }

  comments(id: TicketId): Observable<ApiResponse<TicketComment[]>> {
    return this.http.get<ApiResponse<TicketComment[]>>(SUPPORT_TICKET_COMMENTS_API(id));
  }

  addComment(id: TicketId, message: string): Observable<ApiResponse<TicketComment>> {
    return this.http.post<ApiResponse<TicketComment>>(SUPPORT_TICKET_COMMENTS_API(id), { message });
  }
}
