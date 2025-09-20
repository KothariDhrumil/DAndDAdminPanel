export interface TenantSummary {
  id: number;
  fullName: string;
  shortName: string;
  email: string;
  phone: string;
  address: string;
  status: 'Active' | 'Inactive' | string;
}

export interface ActivePlanSummary {
  name: string;
  startedOn: string; // ISO date
  expiresOn: string; // ISO date
  validityDays: number;
  rate: number; // currency amount
  isAutoRenew: boolean;
}
