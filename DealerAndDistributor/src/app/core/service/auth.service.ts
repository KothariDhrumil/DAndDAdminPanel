import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import {
  BehaviorSubject,
  catchError,
  Observable,
  tap,
  throwError,
} from 'rxjs';

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

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  user$ = new BehaviorSubject<User>({});
 private currentUserTokenSource!: BehaviorSubject<string>;
  public currentUserToken$!: Observable<string>;
  constructor(
    private store: LocalStorageService,
    private toastrService: ToastrService,
    private router: Router,
    private http: HttpClient
  ) {
    this.currentUserTokenSource = new BehaviorSubject<string>(this.store.get('token')?.toString() ?? '');
    this.currentUserToken$ = this.currentUserTokenSource.asObservable();
  }

  public get currentUserValue(): User {
    return this.store.get('currentUser');
  }


  public get getToken(): string {
    return this.store.get('token')?.toString() ?? '';
  }
  /**
   * New sign-in method supporting password and OTP login
   * @param request SigninRequest
   * Returns Observable<ApiResponse<Token>>
   */
  signin(request: SigninRequest): Observable<ApiResponse<Token>> {
    return this.http.post<ApiResponse<Token>>(LOGIN_API, request).pipe(
      tap((response: ApiResponse<Token>) => {
        this.setToken(response);
      }),
      catchError((error) => {
        this.toastrService.error('Login failed');
        return throwError(() => error);
      })
    );
  }
  

  public get isAuthenticated(): boolean {
    const token = this.store.get('token');
    if (typeof token === 'string' && token.trim() !== '') {
      const jwtService = new JwtHelperService();
      return !jwtService.isTokenExpired(token);
    }
    return false;
  }

  public get isSuperAdmin(): boolean {
    const isSuperAdmin = this.store.get('isSuperAdmin');
    return isSuperAdmin === true || isSuperAdmin === 'true';
  }

  private get getStorageRefreshToken(): string {
    return this.store.get('refreshToken') ?? '';
  }


  logout() {
    // remove user from local storage to log user out
    this.store.clear();
    this.router.navigateByUrl(LOGIN_ROUTE);
  }

  public tryRefreshingToken(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwtToken = this.store.get('token') ?? '';
    const refreshToken = this.store.get('refreshToken') ?? '';

    if (!jwtToken || !refreshToken) {
      this.logout();
      return throwError(() => new Error('No token or refresh token found'));
    }
    if (new JwtHelperService().isTokenExpired(jwtToken)) {
      this.toastrService.clear();
      this.toastrService.info('Refreshing token...');
    }
    this.http.post<Token>(REFRESH_TOKEN_API, {
      'refreshToken': refreshToken,
      'token': jwtToken
    })
      .pipe(
        tap((result: Token) => {
          this.store.set('token', result.token);
          this.store.set('refreshToken', result.refreshToken);
          this.store.set('refreshTokenExpiryTime', result.refreshTokenExpiryTime);
          request = request.clone({
            setHeaders: {
              Authorization: 'Bearer ' + result.refreshToken,
            },
          });
          return next.handle(request);
        }),
        catchError((error) => {
          console.error(error);
          this.logout();
          return throwError(() => error);
        })
      );
    return next.handle(request);
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
        this.setToken(response);
      }),
      catchError((error) => {
        this.toastrService.error('Login failed');
        return throwError(() => error);
      })
    );
  }

  private setToken(response: ApiResponse<Token>) {
    if (response.isSuccess && response.data) {
      this.store.set('token', response.data.token);
      this.store.set('refreshToken', response.data.refreshToken);
      this.store.set('refreshTokenExpiryTime', response.data.refreshTokenExpiryTime);
      this.toastrService.clear();
      this.toastrService.info('User Logged In');
    } else {
      this.toastrService.error(response.error?.description || 'Login failed');
    }
  }
  /**
   * Register a new user
   */
  register(request: RegisterRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(REGISTER_API, request);
  }
}
