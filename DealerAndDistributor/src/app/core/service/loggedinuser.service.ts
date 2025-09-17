import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { USER_INFO_API, USER_PERMISSIONS_API } from '../helpers/routes/api-endpoints';
import { ApiResponse } from '../models/interface/ApiResponse';
import { authUser } from "../models/interface/authUser";
import { userInfo } from '../models/interface/LoggedInUserInfo';


@Injectable({
  providedIn: 'root',
})
export class LoggedInUserService {

  constructor(private http: HttpClient) { }

  getUserInfo(): Observable<userInfo> {
    return this.http.get<ApiResponse<userInfo>>(USER_INFO_API).pipe(
      map(response => response.data)
    );
  }

  getPermissions(): Observable<string[]> {
    return this.http.get<ApiResponse<string[]>>(USER_PERMISSIONS_API).pipe(
      map(response => response.data)
    );
  }
}
