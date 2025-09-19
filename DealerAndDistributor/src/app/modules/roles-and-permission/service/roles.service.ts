import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseBase, PaginatedApiResponse } from '../../../core/models/interface/ApiResponse';
import { API_ROLES } from '../../../core/helpers/routes/api-endpoints';
import { ApiRequest } from '../../../core/models/interface/ApiRequest';
import { toApiRequestParams } from '../../../core/helpers/http/to-api-request-params';
import { Role } from '../models/role.model';
import { RoleDetail } from '../models/role-detail.model';


@Injectable({ providedIn: 'root' })
export class RolesService {
 
  
  constructor(private readonly http: HttpClient) {}

  getRoles(pageNumber: number, pageSize: number): Observable<PaginatedApiResponse<Role[]>> {
    const request: ApiRequest = {
      page: {
        pageIndex: pageNumber - 1, // Convert from 1-based to 0-based index
        pageSize: pageSize
      }
    };
    const params = toApiRequestParams(request);
    return this.http.get<PaginatedApiResponse<Role[]>>(API_ROLES, { params });
  }
  createRole(role: RoleDetail): Observable<ApiResponseBase> {
    return this.http.post<ApiResponseBase>(API_ROLES, role);
  }

  updateRole(roleId: number, role: RoleDetail): Observable<ApiResponseBase> {
    return this.http.put<ApiResponseBase>(`${API_ROLES}/${roleId}`, role);
  }

  getRoleById(roleId: number): Observable<ApiResponse<RoleDetail>> {
    return this.http.get<ApiResponse<RoleDetail>>(`${API_ROLES}/${roleId}`);
  }
   deleteRole(roleId: string) {
      return this.http.delete<ApiResponseBase>(`${API_ROLES}/${roleId}`);
  }
}
