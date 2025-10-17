import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs'; 
import { AuthUserItem } from '../models/tenant-user.model';
import { ApiResponse, PaginatedApiResponse } from '@core/models/interface/ApiResponse';
import { AUTHUSERS_LIST_API, AUTHUSERS_API, AUTHUSERS_UPDATE_TENANT_API } from '@core/helpers/routes/api-endpoints';

export interface ListUsersRequest {
  pageNumber?: number;
  pageSize?: number;
  orderBy?: string;
}

export interface CreateUserRequest {
  email: string;
  userName: string;
  tenantId: number;
  password: string;
  isPersistent: boolean;
  firstName: string;
  lastName: string;
  phoneNumber: string;
}

export interface UpdateUserRequest {
  userId: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  roleIds?: number[];
  tenantId?: number;
}

@Injectable({ providedIn: 'root' })
export class TenantUsersService {
  private http = inject(HttpClient);

  listUsers(req: ListUsersRequest = { pageNumber: 1, pageSize: 10, orderBy: 'userName' }): Observable<PaginatedApiResponse<AuthUserItem[]>> {
    let params = new HttpParams();
    if (req.pageNumber) params = params.set('pageNumber', req.pageNumber);
    if (req.pageSize) params = params.set('pageSize', req.pageSize);
    if (req.orderBy) params = params.set('orderBy', req.orderBy);
    return this.http.get<PaginatedApiResponse<AuthUserItem[]>>(AUTHUSERS_LIST_API, { params });
  }

  listUsersByTenant(tenantId: number, req: ListUsersRequest = { pageNumber: 1, pageSize: 10, orderBy: 'userName' }): Observable<PaginatedApiResponse<AuthUserItem[]>> {
    let params = new HttpParams();

    if (req.pageNumber) params = params.set('pageNumber', req.pageNumber);
    if (req.pageSize) params = params.set('pageSize', req.pageSize);
    if (req.orderBy) params = params.set('orderBy', req.orderBy);
    return this.http.get<PaginatedApiResponse<AuthUserItem[]>>(`${AUTHUSERS_LIST_API}/${tenantId}`, { params });
  }

  createUser(payload: CreateUserRequest): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(AUTHUSERS_API, payload);
  }

  updateUser(payload: UpdateUserRequest): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(AUTHUSERS_API, payload);
  }

  updateNameUser(payload: UpdateUserRequest): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(AUTHUSERS_UPDATE_TENANT_API, payload);
  }
}
