// Request model for register

export interface RegisterRequest {
  phoneNumber: string;
  password: string;
  tenantName: string;
  firstName: string;
  lastName: string;
  designationId: number;
}

