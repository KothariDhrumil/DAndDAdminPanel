import { Injectable } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err) => {
        // Do not auto-logout on 401/403 here; let JwtInterceptor handle refresh/retry
        // Surface a normalized error message to callers
        const error = (err?.error?.message) ?? (err?.message) ?? (err?.statusText) ?? 'Unknown error';
        return throwError(() => err ?? error);
      })
    );
  }
}
