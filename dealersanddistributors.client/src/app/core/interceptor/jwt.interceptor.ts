import { Inject, Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { AuthService } from "../service/auth.service";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    request = this.addToken(request);
    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401 || error.status === 403) {
          return this.authService.tryRefreshingToken(request, next).pipe(catchError(() => {  // Error handling for token refresh failure
            this.authService.logout();
            return next.handle(request);

          }));
        }
        return throwError(() => error);
      })
    );
  }

  private addToken(request: HttpRequest<any>) {
    if (this.authService.isAuthenticated) {
      const localToken = this.authService.getToken;
      request = request.clone({ setHeaders: { 'Authorization': `Bearer ${localToken}` } });
    }
    return request;
  }
}
