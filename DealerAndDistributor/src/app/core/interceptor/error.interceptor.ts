import { Injectable, inject } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError, of } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { MatDialog } from '@angular/material/dialog';
import { ErrorTicketComponent } from "../shared/components/error-ticket/error-ticket.component";
import { LocalStorageService } from "../shared/services";
import { SupportTicketPayload } from "../shared/services/support-ticket.service";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private dialog = inject(MatDialog);
  private store = inject(LocalStorageService);
  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        // Do not auto-logout on 401/403 here; let JwtInterceptor handle refresh/retry
        const errorMsg = (err?.error?.message) ?? (err?.message) ?? (err?.statusText) ?? 'Unknown error';

        // Build support ticket payload
        const nowIso = new Date().toISOString();
        const userAgent = typeof navigator !== 'undefined' ? navigator.userAgent : 'server';
        const appVersion = this.store.get('app_version') as string | undefined;
        const correlationId = Math.random().toString(36).slice(2) + '-' + Date.now();
        const payload: SupportTicketPayload = {
          message: errorMsg,
          url: request.url,
          method: request.method,
          status: err?.status,
          statusText: err?.statusText,
          userAgent,
          timestamp: nowIso,
          appVersion: typeof appVersion === 'string' ? appVersion : undefined,
          requestBody: request?.body,
          responseBody: err?.error,
          headers: request.headers?.keys()?.reduce((acc: any, k: string) => { acc[k] = request.headers.get(k); return acc; }, {} as any),
          correlationId,
        };

        // Open a lightweight dialog to let user submit a ticket
        try {
          this.dialog.open(ErrorTicketComponent, {
            width: '560px',
            // Pass full payload for backend submission; the dialog template only shows non-sensitive fields
            data: { payload },
          });
        } catch {
          // If dialog fails (e.g., in non-browser), ignore
        }

        return throwError(() => err ?? errorMsg);
      })
    );
  }
}
0