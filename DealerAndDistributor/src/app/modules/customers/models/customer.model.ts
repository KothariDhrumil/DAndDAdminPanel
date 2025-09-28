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

export interface CustomerQuery {
  pageNumber?: number;
  pageSize?: number;
  filter?: string;
  sort?: string;
}
