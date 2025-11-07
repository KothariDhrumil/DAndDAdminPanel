import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TENANTS_API, TENANTS_CHILDREN_API, TENANTS_CREATE_API } from '../../../../core/helpers/routes/api-endpoints';
import { ApiResponse, ApiResponseBase, PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';
import { ApiRequest } from '../../../../core/models/interface/ApiRequest';
import { toApiRequestParams } from '../../../../core/helpers/http/to-api-request-params';
import { Tenant } from '../models/tenant.model';

@Injectable({ providedIn: 'root' })
export class TenantsService {
  constructor(private http: HttpClient) { }

  getTenants(request?: ApiRequest): Observable<PaginatedApiResponse<Tenant[]>> {
    const params = toApiRequestParams(request);
    return this.http.get<PaginatedApiResponse<Tenant[]>>(TENANTS_API, { params });
  }

  getChildTenants(parentTenantId: number): Observable<ApiResponse<Tenant[]>> {
    return this.http.get<ApiResponse<Tenant[]>>(TENANTS_CHILDREN_API(parentTenantId));
  }

  createChildTenant(parentTenantId: number, tenantName: string): Observable<ApiResponseBase> {
    const payload = { tenantName, parentId: parentTenantId };
    return this.http.post<ApiResponseBase>(TENANTS_CREATE_API, payload);
  }

  createTenant(tenantName: string): Observable<ApiResponseBase> {
    const payload = { tenantName };
    return this.http.post<ApiResponseBase>(TENANTS_CREATE_API, payload);
  }

  renameTenant(tenantId: number, tenantName: string): Observable<ApiResponseBase> {
    const payload = { tenantId, tenantName };
    return this.http.put<ApiResponseBase>(TENANTS_API, payload);
  }
}
