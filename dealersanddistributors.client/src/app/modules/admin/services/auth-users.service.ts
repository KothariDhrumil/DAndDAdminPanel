import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { PaginatedResult } from '../../../core/models/wrappers/PaginatedResult';
import { UserParams } from '../models/userParams';
import { AuthUserInfo, SyncAuthUserWithChange } from '../models/authuserinfo.model';

@Injectable({
  providedIn: 'root'
})
export class AuthUsersService {


  baseUrl = environment.apiUrl + 'authusers/';

  constructor(private http: HttpClient) {
  }

  getSyncUsers(UserParams: UserParams): Observable<PaginatedResult<SyncAuthUserWithChange>> {
    return this.http.get<PaginatedResult<SyncAuthUserWithChange>>(this.baseUrl + 'view-sync-changes')
      .pipe(map((response: PaginatedResult<SyncAuthUserWithChange>) => response));
  }

  getUsers(UserParams: UserParams): Observable<PaginatedResult<AuthUserInfo>> {
    let params = new HttpParams();
    if (UserParams.searchString)
      params = params.append('searchString', UserParams.searchString);
    if (UserParams.pageNumber)
      params = params.append('pageNumber', UserParams.pageNumber.toString());
    if (UserParams.pageSize)
      params = params.append('pageSize', UserParams.pageSize.toString());
    if (UserParams.orderBy)
      params = params.append('orderBy', UserParams.orderBy.toString());
    return this.http.get<PaginatedResult<AuthUserInfo>>(this.baseUrl + 'listusers', { params: params })
      .pipe(map((response: PaginatedResult<AuthUserInfo>) => response));
  }
  deleteUser(userId: string): Observable<any> {
    return this.http.delete(this.baseUrl + userId);
  }

  addUser(user: AuthUserInfo): Observable<any> {
    return this.http.post(this.baseUrl, user);
  }

  updateUser(user: AuthUserInfo): Observable<any> {
    return this.http.put(this.baseUrl, user);
  }

  getUser(userId: string): Observable<AuthUserInfo> {
    return this.http.get<AuthUserInfo>(this.baseUrl + userId);
  }

  updateUserRole(userId: string, roles: {}) {
    return this.http.put(this.baseUrl + userId + '/roles', roles);
  }

  applySyncUsers(users: SyncAuthUserWithChange[]): Observable<any> {
    return this.http.post(this.baseUrl + 'apply-sync-changes', users);
  }
}
