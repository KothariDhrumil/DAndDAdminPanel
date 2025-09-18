import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TENANTS_API } from '../../../../core/helpers/routes/api-endpoints';
import { TenantsResponse } from '../models/tenants-response.model';
import { PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';
import { ApiRequest } from '../../../../core/models/interface/ApiRequest';
import { toApiRequestParams } from '../../../../core/helpers/http/to-api-request-params';

@Injectable({ providedIn: 'root' })
export class TenantsService {
  constructor(private http: HttpClient) { }

  getTenants(request?: ApiRequest): Observable<PaginatedApiResponse<TenantsResponse>> {
    const params = toApiRequestParams(request);
    return this.http.get<PaginatedApiResponse<TenantsResponse>>(TENANTS_API, { params });
  }
}
