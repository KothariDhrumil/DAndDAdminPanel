export interface User {
  [prop: string]: any;

  id?: number | string | null;
  name?: string;
  email?: string;
  avatar?: string;
  roles?: any[];
  permissions?: any[];
}

export interface Token {

  token: string;
  refreshToken: string;
  refreshTokenExpiryTime: Date;
}
