import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import {
  BehaviorSubject,
  catchError,
  merge,
  Observable,
  of,
  share,
  switchMap,
  tap,
  throwError,
} from 'rxjs';

import { LoginService } from './login.service';
import { Token, User } from '../models/interface';
import { LocalStorageService } from '../shared/services';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LOGIN_ROUTE } from '../helpers/routes/app-routes';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  user$ = new BehaviorSubject<User>({});
  private baseUrl = environment.apiUrl;
  private currentUserTokenSource!: BehaviorSubject<string>;
  public currentUserToken$!: Observable<string>;
  constructor(
    private loginService: LoginService,
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

  login(email: string, password: string, rememberMe = false) {
    return this.loginService.login(email, password).pipe(
      switchMap((response) => {
        if (response) {
          this.toastrService.clear();
          this.toastrService.info('User Logged In');
        } else {
          this.toastrService.error('Login failed');
        }
        return of(response); // Return the response to be handled in the component
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
    this.http.post<Token>(this.baseUrl + '/tokens/refresh', {
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
}
