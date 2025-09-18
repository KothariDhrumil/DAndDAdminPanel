import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sharding, ShardingRequest } from '../models/sharding.model';
import { API_SHARDING } from '../../../../core/helpers/routes/api-endpoints';
import { ApiResponse } from '../../../../core/models/interface/ApiResponse';

@Injectable({ providedIn: 'root' })
export class ShardingService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<Sharding[]>> {
    return this.http.get<ApiResponse<Sharding[]>>(API_SHARDING);
  }

  create(data: ShardingRequest): Observable<ApiResponse<Sharding>> {
    return this.http.post<ApiResponse<Sharding>>(API_SHARDING, data);
  }

  update(id: string, data: ShardingRequest): Observable<ApiResponse<Sharding>> {
    return this.http.put<ApiResponse<Sharding>>(`${API_SHARDING}/${id}`, data);
  }

  delete(id: string): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${API_SHARDING}/${id}`);
  }
}
