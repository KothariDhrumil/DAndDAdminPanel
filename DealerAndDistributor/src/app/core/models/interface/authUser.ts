export interface authUser {
  userName: string;
  email: string;
  userId: string;
  roleNames: string[];
  tenantFeatures: string[];
  hasTenant: boolean;
  tenantName: string;
}

export interface user {

  designationId?: number;
  firstName?: string; 
  lastName?: string;
  phoneNumber?: string;
  
}
