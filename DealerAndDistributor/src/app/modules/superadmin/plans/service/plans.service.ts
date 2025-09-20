import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_PLANS } from '../../../../core/helpers/routes/api-endpoints';
import { ApiResponse, ApiResponseBase, PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';
import { Plan, PlanRequest } from '../models/plan.model';

@Injectable({ providedIn: 'root' })
export class PlansService {
  constructor(private readonly http: HttpClient) {}

  list(): Observable<ApiResponse<Plan[]>> {
    return this.http.get<ApiResponse<Plan[]>>(API_PLANS);
  }

  getById(id: number): Observable<ApiResponse<Plan>> {
    return this.http.get<ApiResponse<Plan>>(`${API_PLANS}/${id}`);
  }

  create(payload: PlanRequest): Observable<ApiResponseBase> {
    return this.http.post<ApiResponseBase>(API_PLANS, payload);
  }

  update(id: number, payload: PlanRequest): Observable<ApiResponseBase> {
    return this.http.put<ApiResponseBase>(`${API_PLANS}/${id}`, payload);
  }

  delete(id: number): Observable<ApiResponseBase> {
    return this.http.delete<ApiResponseBase>(`${API_PLANS}/${id}`);
  }
}
