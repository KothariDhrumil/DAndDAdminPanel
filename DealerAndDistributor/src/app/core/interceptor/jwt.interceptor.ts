import { Injectable } from "@angular/core";
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpStatusCode } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { AuthService } from "../service/auth.service";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
   request = this.addToken(request);
    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === HttpStatusCode.Unauthorized || error.status === HttpStatusCode.Forbidden) {
          return this.authService
            .tryRefreshingToken(request, next)
            .pipe(
              catchError((refreshError) => {
                this.authService.logout();
                return throwError(() => refreshError);
              })
            );
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
