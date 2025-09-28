export interface AuthUserItem {
  userName: string;
  email: string;
  userId: string;
  roleNames: string[];
  tenantFeatures: string[] | null;
  hasTenant: boolean;
  tenantName: string | null;
}
