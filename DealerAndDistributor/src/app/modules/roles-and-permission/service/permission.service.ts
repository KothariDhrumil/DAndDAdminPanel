import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PermissionApiResponse } from '../add-role/models/permission.model';
import { API_ROLES } from '../../../core/helpers/routes/api-endpoints';

@Injectable({ providedIn: 'root' })
export class PermissionService {
  constructor(private http: HttpClient) {}

  getPermissions(pageNumber: number = 1, pageSize: number = 100): Observable<PermissionApiResponse> {
    return this.http.get<PermissionApiResponse>(`${API_ROLES}/permissions?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
