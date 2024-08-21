import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Tenants } from "../models/tenants.model";

@Injectable({
  providedIn: 'root'
})
export class TenantsService {


  baseUrl = environment.apiUrl + 'tenants/';

  constructor(private http: HttpClient) {
  }

  getTenants() {
    return this.http.get<Tenants[]>(this.baseUrl);
  }

  addTenant(tenant: any) {
    return this.http.post(this.baseUrl, tenant);
  }

}
