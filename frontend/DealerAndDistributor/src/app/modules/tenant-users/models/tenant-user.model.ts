export interface AuthUserItem {
  userName: string;
  email: string;
  userId: string;
  roleNames: string[];
  tenantFeatures: string[] | null;
  hasTenant: boolean;
  tenantName: string | null;
  firstName: string | null;
  lastName: string | null;
  phoneNumber: string | null;
}
