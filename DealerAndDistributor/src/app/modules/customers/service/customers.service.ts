import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CustomerWithTenants, TenantCustomerProfile } from '../models/customer.model';
import { CUSTOMERS_WITH_TENANTS_API, CUSTOMERS_BY_TENANT_API, CUSTOMERS_SEARCH_BY_PHONE_API, CUSTOMERS_LINK_API, CUSTOMERS_API, CUSTOMERS_TENANT_PROFILE_API, CUSTOMERS_CHILD_API, CUSTOMERS_CHILD_LINK_API, CUSTOMER_ORDERS_API } from '../../../core/helpers/routes/api-endpoints';
import { PaginatedApiResponse } from '../../../core/models/interface/ApiResponse';
import { ApiResponse } from '../../../core/models/interface/ApiResponse';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  constructor(private http: HttpClient) {}

  getCustomersWithTenants(pageNumber: number, pageSize: number): Observable<PaginatedApiResponse<CustomerWithTenants[]>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PaginatedApiResponse<CustomerWithTenants[]>>(CUSTOMERS_WITH_TENANTS_API, { params });
  }

  getCustomersByTenant(pageNumber: number, pageSize: number): Observable<PaginatedApiResponse<TenantCustomerProfile[]>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PaginatedApiResponse<TenantCustomerProfile[]>>(CUSTOMERS_TENANT_PROFILE_API, { params });
  }
   
  searchByPhone(phone: string): Observable<ApiResponse<CustomerWithTenants | null>> {
    return this.http.get<ApiResponse<CustomerWithTenants | null>>(CUSTOMERS_SEARCH_BY_PHONE_API(phone));
  }

  linkExistingCustomer(globalCustomerId: string, phoneNumber: string, parentGlobalCustomerId?: string | null): Observable<ApiResponse<any>> {
    const url = parentGlobalCustomerId ? CUSTOMERS_CHILD_LINK_API : CUSTOMERS_LINK_API;
    return this.http.post<ApiResponse<any>>(url, { globalCustomerId, phoneNumber, parentGlobalCustomerId });
  }

  createCustomer(payload: { phoneNumber: string; firstName: string; lastName: string; parentGlobalCustomerId?: string }): Observable<ApiResponse<any>> {
    const url = payload.parentGlobalCustomerId ? CUSTOMERS_CHILD_API : CUSTOMERS_API;
    return this.http.post<ApiResponse<any>>(url, payload);
  }

  // Orders
  createCustomerOrder(globalCustomerId: string, total: number): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(CUSTOMER_ORDERS_API, { globalCustomerId, total });
  }

  getCustomerOrders(globalCustomerId: string): Observable<ApiResponse<any[]>> {
    const params = new HttpParams().set('globalCustomerId', globalCustomerId);
    return this.http.get<ApiResponse<any[]>>(CUSTOMER_ORDERS_API, { params });
  }
}
