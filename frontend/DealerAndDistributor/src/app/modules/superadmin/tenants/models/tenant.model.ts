export interface Tenant {
  tenantId: number;
  tenantFullName: string;
  tenantName: string;
  dataKey: string;
  hasOwnDb: boolean;
  databaseInfoName: string;
  listOfTenants: unknown;
  parentId: number;
}
