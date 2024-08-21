import { PaginatedResult } from './../../../core/models/wrappers/PaginatedResult';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { AuthRole } from "../models/role.model";
import { Permission } from '../models/permission.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {


  baseUrl = environment.apiUrl + 'roles/';

  constructor(private http: HttpClient) {
  }

  getRoles() {
    return this.http.get<PaginatedResult<AuthRole>>(this.baseUrl);
  }

  addRole(role: any) {
    return this.http.post(this.baseUrl, role);
  }

  updateRole(role: any) {
    return this.http.put(this.baseUrl +'permissions', role);
  }

  deleteRole(roleId: string) {
    return this.http.delete(this.baseUrl + roleId);
  }

  getPermissions() {
    return this.http.get<PaginatedResult<Permission>>(this.baseUrl + 'permissions');
  }
}
