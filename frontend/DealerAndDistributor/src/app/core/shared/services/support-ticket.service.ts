import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SUPPORT_TICKETS_API } from '../../helpers/routes/api-endpoints';
import { Observable } from 'rxjs';

export interface SupportTicketPayload {
  message: string;
  url: string;
  method: string;
  status?: number;
  statusText?: string;
  userAgent: string;
  timestamp: string;
  appVersion?: string;
  stack?: string;
  requestBody?: any;
  responseBody?: any;
  headers?: any;
  correlationId?: string;
  
}

@Injectable({ providedIn: 'root' })
export class SupportTicketService {
  constructor(private http: HttpClient) {}

  raise(payload: SupportTicketPayload): Observable<any> {
    return this.http.post(SUPPORT_TICKETS_API, payload);
  }
}
