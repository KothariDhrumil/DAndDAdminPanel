import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CustomerWithTenants } from '../models/customer.model';
import { CUSTOMERS_WITH_TENANTS_API, CUSTOMERS_BY_TENANT_API, CUSTOMERS_SEARCH_BY_PHONE_API, CUSTOMERS_LINK_API, CUSTOMERS_API } from '../../../core/helpers/routes/api-endpoints';
import { PaginatedApiResponse } from '../../../core/models/interface/ApiResponse';
import { ApiResponse } from '../../../core/models/interface/ApiResponse';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  constructor(private http: HttpClient) {}

  getCustomersWithTenants(pageNumber: number, pageSize: number): Observable<PaginatedApiResponse<CustomerWithTenants[]>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PaginatedApiResponse<CustomerWithTenants[]>>(CUSTOMERS_WITH_TENANTS_API, { params });
  }

  getCustomersByTenant(tenantId: number, pageNumber: number, pageSize: number): Observable<PaginatedApiResponse<CustomerWithTenants[]>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PaginatedApiResponse<CustomerWithTenants[]>>(CUSTOMERS_BY_TENANT_API(tenantId), { params });
  }

  searchByPhone(phone: string): Observable<ApiResponse<CustomerWithTenants | null>> {
    return this.http.get<ApiResponse<CustomerWithTenants | null>>(CUSTOMERS_SEARCH_BY_PHONE_API(phone));
  }

  linkExistingCustomer(globalCustomerId: string, phoneNumber: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(CUSTOMERS_LINK_API, { globalCustomerId, phoneNumber });
  }

  createCustomer(payload: { phoneNumber: string; firstName: string; lastName: string }): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(CUSTOMERS_API, payload);
  }
}
