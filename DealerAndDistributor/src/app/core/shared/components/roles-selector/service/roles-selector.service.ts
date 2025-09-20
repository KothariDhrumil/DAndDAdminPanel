import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../../../models/interface/ApiResponse';
import { API_ROLES_BY_TYPE } from '../../../../helpers/routes/api-endpoints';
import { RoleOption } from '../models/role-option.model';

interface RoleApiItem {
  roleId: number | string;
  roleName: string;
  description?: string;
}

@Injectable({ providedIn: 'root' })
export class RolesSelectorService {
  private readonly http = inject(HttpClient);

  getRolesByType(roleTypes: number): Observable<RoleOption[]> {
    const params = new HttpParams().set('roleTypes', roleTypes.toString());
    return this.http.get<ApiResponse<RoleApiItem[]>>(API_ROLES_BY_TYPE, { params }).pipe(
      map(res => (res.data || []).map(r => ({ id: Number(r.roleId), name: r.roleName, description: r.description })))
    );
  }
}
