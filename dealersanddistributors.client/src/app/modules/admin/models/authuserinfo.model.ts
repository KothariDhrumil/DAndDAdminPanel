

export class AuthUserInfo {

  userName: string = '';
  email: string = '';

  userId !: string;

  roleNames: string[] = [];

  hasTenant: boolean = false;

  tenantName: string = '';

  foundChangeType !: foundChangeType;
}

export class SyncAuthUserWithChange extends AuthUserInfo {
  oldEmail: string = '';
  emailChanged: boolean = false;
  oldUsername: string = '';
  usernameChanged: boolean = false;
  NumRoles: number = 0;
}

enum foundChangeType {
  NoChange = 0,
  Create = 1,
  Update = 2,
  Delete = 3
}

export { foundChangeType };
