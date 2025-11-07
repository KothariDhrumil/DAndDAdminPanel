export interface CustomerTenantRef {
  tenantId: number;
  tenantName: string;
}

export interface CustomerWithTenants {
  globalCustomerId: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  tenants: CustomerTenantRef[];
}



export interface TenantCustomerProfile {
  globalCustomerId: string;
  tenantCustomerId : number;
  phoneNumber: string;
   firstName: string;
  lastName: string;
  tenantId: number;
}



export interface CustomerQuery {
  pageNumber?: number;
  pageSize?: number;
  filter?: string;
  sort?: string;
}
