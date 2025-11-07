export interface Plan {
  id: number;
  name: string;
  description?: string;
  planValidityInDays: number;
  planRate: number;
  isActive: boolean;
  roleIds: number[];
}

export interface PlanRequest {
  name: string;
  description?: string;
  planValidityInDays: number;
  planRate: number;
  isActive: boolean;
  roleIds: number[];
  planId: number; // For backward compatibility with some APIs
}
