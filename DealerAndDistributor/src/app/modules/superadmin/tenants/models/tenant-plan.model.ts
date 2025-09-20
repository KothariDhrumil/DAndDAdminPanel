export interface TenantPlanItem {
  id: number;
  tenantPlanId: number;
  isActive: boolean;
  validFrom: string; // ISO date (yyyy-MM-dd or full ISO)
  validTo: string;   // ISO date (yyyy-MM-dd or full ISO)
  remarks: string;
  planName: string;
  planRate: number;
  createdOn: string; // ISO date
}

export interface TenantPlanUpsertRequest {
  planId: number;
  tenantId: number;
  isActive: boolean;
  validFrom: string; // server expects date in string format
  validTo: string;   // server expects date in string format
  remarks?: string;
  roles: number[];
}
