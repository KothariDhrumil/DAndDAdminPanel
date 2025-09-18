import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TENANTS_API } from '../../../../core/helpers/routes/api-endpoints';
import { TenantsResponse } from '../models/tenants-response.model';
import { PaginatedApiResponse } from '../../../../core/models/interface/ApiResponse';

@Injectable({ providedIn: 'root' })
export class TenantsService {
  constructor(private http: HttpClient) {}

  getTenants(): Observable<PaginatedApiResponse<TenantsResponse>> {
    return this.http.get<PaginatedApiResponse<TenantsResponse>>(TENANTS_API);
  }
}
