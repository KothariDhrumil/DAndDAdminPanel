import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ApiResponse, ApiResponseBase } from '../../../../core/models/interface/ApiResponse';
import { TENANTS_API, TENANT_PLAN_ACTIVE_API, TENANT_PLAN_API } from '../../../../core/helpers/routes/api-endpoints';
import { Tenant } from '../models/tenant.model';
import { TenantPlanItem, TenantPlanUpsertRequest } from '../models/tenant-plan.model';

@Injectable({ providedIn: 'root' })
export class TenantDetailService {
  constructor(private http: HttpClient) { }

  getTenantById(tenantId: number): Observable<ApiResponse<Tenant>> {
    return this.http.get<ApiResponse<Tenant>>(`${TENANTS_API}/${tenantId}`);
  }

  getActivePlan(tenantId: number): Observable<ApiResponse<TenantPlanItem>> {
    return this.http.get<ApiResponse<TenantPlanItem>>(`${TENANT_PLAN_ACTIVE_API}/${tenantId}`);
  }

  // API spec says list of all plan history at /api/tenantplan/:{tenantPlan}
  // Assuming the response is a single item for now (as per sample). If API returns array later, adjust type.
  getPlanHistory(tenantPlanId: number): Observable<ApiResponse<TenantPlanItem>> {
    return this.http.get<ApiResponse<TenantPlanItem>>(`${TENANT_PLAN_API}/${tenantPlanId}`);
  }

  createTenantPlan(payload: TenantPlanUpsertRequest): Observable<ApiResponseBase> {
    return this.http.post<ApiResponseBase>(TENANT_PLAN_API, payload);
  }

  updateTenantPlan(
    tenantPlanId: number,
    payload: TenantPlanUpsertRequest
  ): Observable<ApiResponseBase> {
    // Using PUT on /tenantplan/{tenantPlanId}
    return this.http.put<ApiResponseBase>(`${TENANT_PLAN_API}/${tenantPlanId}`, payload);
  }
}
