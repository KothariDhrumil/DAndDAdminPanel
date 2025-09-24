import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError, switchMap, Subscription, timer } from 'rxjs';
import { environment } from '../../../environments/environment';

import { Token } from "../models/interface/Token";
import { ApiResponse } from '../models/interface/ApiResponse';

import { User } from "../models/interface/User";
import { LocalStorageService } from '../shared/services';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { LOGIN_ROUTE } from '../helpers/routes/app-routes';
import { CONFIRM_OTP_API, GENERATE_OTP_API, LOGIN_API, REFRESH_TOKEN_API, REGISTER_API } from '../helpers/routes/api-endpoints';
import { SigninRequest } from '../models/interface/SigninRequest';
import { RegisterRequest } from '../models/interface/RegisterRequest';
import { UpsertTenantFormValue } from '@core/shared/components/upsert-tenant/upsert-tenant.component';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  user$ = new BehaviorSubject<User>({});
 private currentUserTokenSource!: BehaviorSubject<string>;
  public currentUserToken$!: Observable<string>;
  private refreshSub?: Subscription;
  constructor(
    private store: LocalStorageService,
    private toastrService: ToastrService,
    private router: Router,
    private http: HttpClient
  ) {
  this.currentUserTokenSource = new BehaviorSubject<string>(this.store.getAuthItem<string>('token') ?? '');
    this.currentUserToken$ = this.currentUserTokenSource.asObservable();
    // schedule refresh if already authenticated on startup
    if (this.isAuthenticated && environment.tokenAutoRefreshEnabled) {
      this.scheduleAutoRefresh();
    }
  }

  public get currentUserValue(): User {
    return this.store.get('currentUser');
  }


  public get getToken(): string {
    return this.store.getAuthItem<string>('token') ?? '';
  }
  /**
   * New sign-in method supporting password and OTP login
   * @param request SigninRequest
   * Returns Observable<ApiResponse<Token>>
   */
  signin(request: SigninRequest, rememberMe = false): Observable<ApiResponse<Token>> {
    // persist preference
    this.store.setRememberMe(rememberMe);
    return this.http.post<ApiResponse<Token>>(LOGIN_API, request).pipe(
      tap((response: ApiResponse<Token>) => {
        this.setToken(response, rememberMe);
      }),
      catchError((error) => {
        this.toastrService.error('Login failed');
        return throwError(() => error);
      })
    );
  }
  

  public get isAuthenticated(): boolean {
    const token = this.store.getAuthItem<string>('token');
    // Ensure token is a non-empty string
    if (typeof token !== 'string') return false;
    const trimmed = token.trim();
    if (!trimmed) return false;
    // Basic JWT format check to avoid library exceptions
    if (trimmed.split('.').length !== 3) return false;
    try {
      const jwtService = new JwtHelperService();
      return !jwtService.isTokenExpired(trimmed);
    } catch {
      return false;
    }
  }

  public get isSuperAdmin(): boolean {
    const isSuperAdmin = this.store.get('isSuperAdmin');
    return isSuperAdmin === true || isSuperAdmin === 'true';
  }

  private get getStorageRefreshToken(): string {
    return this.store.getAuthItem<string>('refreshToken') ?? '';
  }


  logout() {
    // remove user from local storage to log user out
  this.store.clear();
  // also clear auth items from session explicitly
  this.store.removeAuthItem('token');
  this.store.removeAuthItem('refreshToken');
  this.store.removeAuthItem('refreshTokenExpiryTime');

    // TODO : call logout API using HTTPClient and JWT token for server-side logout if required
    


    this.router.navigateByUrl(LOGIN_ROUTE);
    // stop any scheduled refresh timers
    this.refreshSub?.unsubscribe();
    this.refreshSub = undefined;
  }

  public tryRefreshingToken(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  const jwtToken = this.store.getAuthItem<string>('token') ?? '';
  const refreshToken = this.store.getAuthItem<string>('refreshToken') ?? '';

    if (!jwtToken || !refreshToken) {
      this.logout();
      return throwError(() => new Error('No token or refresh token found'));
    }
    if (new JwtHelperService().isTokenExpired(jwtToken)) {
      this.toastrService.clear();
      this.toastrService.info('Refreshing token...');
    }
    return this.http
      .post<Token>(REFRESH_TOKEN_API, { refreshToken, token: jwtToken })
      .pipe(
        tap((result: Token) => {
          const remember = this.store.getRememberMe();
          this.store.setAuthItem('token', result.token, remember);
          this.store.setAuthItem('refreshToken', result.refreshToken, remember);
          this.store.setAuthItem('refreshTokenExpiryTime', result.refreshTokenExpiryTime, remember);
          // reschedule auto-refresh
          if (environment.tokenAutoRefreshEnabled) {
            this.scheduleAutoRefresh();
          }
        }),
        switchMap((result: Token) => {
          const authReq = request.clone({
            setHeaders: { Authorization: 'Bearer ' + result.token },
          });
          return next.handle(authReq);
        }),
        catchError((error) => {
          console.error(error);
          this.logout();
          return throwError(() => error);
        })
      );
  }
   /**
   * Generate OTP for phone number
   */
  generateOTP(phoneNumber: string): Observable<ApiResponse<string>> {
    return this.http.get<ApiResponse<string>>(GENERATE_OTP_API + `?phoneNumber=${encodeURIComponent(phoneNumber)}`);
  }

  /**
   * Confirm OTP for phone number
   */
  confirmOTP(phoneNumber: string, code: string): Observable<ApiResponse<Token>> {
    return this.http.get<ApiResponse<Token>>(CONFIRM_OTP_API + `?phoneNumber=${encodeURIComponent(phoneNumber)}&code=${encodeURIComponent(code)}`)
    .pipe(
      tap((response: ApiResponse<Token>) => {
        this.setToken(response, this.store.getRememberMe());
      }),
      catchError((error) => {
        this.toastrService.error('Login failed');
        return throwError(() => error);
      })
    );
  }

  private setToken(response: ApiResponse<Token>, rememberMe?: boolean) {
    if (response.isSuccess && response.data) {
      // persist according to rememberMe
      this.store.setAuthItem('token', response.data.token, rememberMe);
      this.store.setAuthItem('refreshToken', response.data.refreshToken, rememberMe);
      this.store.setAuthItem('refreshTokenExpiryTime', response.data.refreshTokenExpiryTime, rememberMe);
      this.toastrService.clear();
      this.toastrService.info('User Logged In');
      // schedule auto-refresh
      if (environment.tokenAutoRefreshEnabled) {
        this.scheduleAutoRefresh();
      }
    } else {
      this.toastrService.error(response.error?.description || 'Login failed');
    }
  }

  private scheduleAutoRefresh() {
    // clear previous schedule
    this.refreshSub?.unsubscribe();
    this.refreshSub = undefined;
    const token = this.store.getAuthItem<string>('token');
    if (!token) return;
    const jwt = new JwtHelperService();
    const expDate = jwt.getTokenExpirationDate(token);
    if (!expDate) return;
    const aheadSec = Math.max(0, environment.tokenRefreshAheadSeconds ?? 30);
    const msUntil = expDate.getTime() - Date.now() - aheadSec * 1000;
    if (msUntil <= 0) {
      // expire soon → refresh now by making a noop request to trigger interceptor, or call refresh directly
      const refreshToken = this.store.getAuthItem<string>('refreshToken');
      if (refreshToken) {
        // direct refresh call
        this.http
          .post<Token>(REFRESH_TOKEN_API, { refreshToken, token })
          .pipe(
            tap((result) => {
              const remember = this.store.getRememberMe();
              this.store.setAuthItem('token', result.token, remember);
              this.store.setAuthItem('refreshToken', result.refreshToken, remember);
              this.store.setAuthItem('refreshTokenExpiryTime', result.refreshTokenExpiryTime, remember);
            })
          )
          .subscribe({
            next: () => this.scheduleAutoRefresh(),
            error: () => this.logout(),
          });
      }
      return;
    }
    this.refreshSub = timer(msUntil).subscribe(() => {
      const rt = this.store.getAuthItem<string>('refreshToken');
      const tk = this.store.getAuthItem<string>('token');
      if (!rt || !tk) {
        this.logout();
        return;
      }
      this.http
        .post<Token>(REFRESH_TOKEN_API, { refreshToken: rt, token: tk })
        .pipe(
          tap((result) => {
            const remember = this.store.getRememberMe();
            this.store.setAuthItem('token', result.token, remember);
            this.store.setAuthItem('refreshToken', result.refreshToken, remember);
            this.store.setAuthItem('refreshTokenExpiryTime', result.refreshTokenExpiryTime, remember);
          })
        )
        .subscribe({
          next: () => this.scheduleAutoRefresh(),
          error: () => this.logout(),
        });
    });
  }
  /**
   * Register a new user
   */
  register(request: RegisterRequest | UpsertTenantFormValue): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(REGISTER_API, request);
  }
}
