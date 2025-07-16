import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map, of, tap } from 'rxjs';

import { JWT } from './JWT';
import { Token, User } from '../models/interface';
import { LocalStorageService } from '../shared/services';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
const jwt = new JWT();

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private baseUrl = environment.apiUrl;
  private tokenUrl = this.baseUrl + 'tokens';
  constructor(protected http: HttpClient, private store: LocalStorageService) { }

  login(email: string, password: string) {
    const values = {
      email: email,
      password: password,
    };

    return this.http.post<Token>(this.tokenUrl, values)
      .pipe(
        tap((result: Token) => {
          this.storeToken(result);
        }),
        map((result: Token) => result ?? undefined)
      );
  }
  storeToken(result: Token) {
    if (result) {
      this.store.set('token', result.token);
      this.store.set('refreshToken', result.refreshToken);
      this.store.set('refreshTokenExpiryTime', result.refreshTokenExpiryTime);
    } else {
      this.store.clear();
    }
  }

}
