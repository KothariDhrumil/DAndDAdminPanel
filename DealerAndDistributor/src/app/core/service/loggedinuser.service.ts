import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface LoggedInUserInfo {
  userName: string;
  email: string;
  userId: string;
  roleNames: string[];
  hasTenant: boolean;
  tenantName: string;
}

@Injectable({
  providedIn: 'root',
})
export class LoggedInUserService {

  constructor(private http: HttpClient) { }

  getAuthUserInfo(): Observable<LoggedInUserInfo> {
    return this.http.get<LoggedInUserInfo>(environment.apiUrl + 'loggedinuser/authuserinfo');
  }

  getPermissions(): Observable<string[]> {
    return this.http.get<string[]>(environment.apiUrl + 'loggedinuser/permissions');
  }
}
