import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../models/role.model';
import { PaginatedApiResponse } from '../../../../../core/models/interface/ApiResponse';
import { API_ROLES } from '../../../../../core/helpers/routes/api-endpoints';
import { ApiRequest } from '../../../../../core/models/interface/ApiRequest';
import { toApiRequestParams } from '../../../../../core/helpers/http/to-api-request-params';

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
}
