import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserType, UpsertUserTypeRequest, UserTypeApiResponse } from '../models/user-type.model';
import { API_USER_TYPES } from '../../../../core/helpers/routes/api-endpoints';

@Injectable({ providedIn: 'root' })
export class UserTypeService {
  private http = inject(HttpClient);

  list(): Observable<UserTypeApiResponse> {
    return this.http.get<UserTypeApiResponse>(API_USER_TYPES);
  }

  getById(id: number): Observable<UserTypeApiResponse> {
    return this.http.get<UserTypeApiResponse>(`${API_USER_TYPES}/${id}`);
  }

  create(payload: UpsertUserTypeRequest): Observable<UserTypeApiResponse> {
    return this.http.post<UserTypeApiResponse>(API_USER_TYPES, payload);
  }

  update(id: number, payload: UpsertUserTypeRequest): Observable<UserTypeApiResponse> {
    return this.http.put<UserTypeApiResponse>(`${API_USER_TYPES}/${id}`, payload);
  }

  delete(id: number): Observable<UserTypeApiResponse> {
    return this.http.delete<UserTypeApiResponse>(`${API_USER_TYPES}/${id}`);
  }
}
