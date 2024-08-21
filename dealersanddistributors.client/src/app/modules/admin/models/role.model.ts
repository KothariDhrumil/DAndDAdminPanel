import { FormControl } from "@angular/forms";

export class AuthRoleFormModel {

  roleName: string = '';
  description: string = '';
  permissionsWithSelect: permissionsWithSelect[] = [];
}

export interface permissionsWithSelect {

  permissionName: string;
  selected: boolean;
}


export class AuthRole {

  roleName: string = '';
  description: string = '';
  permissionNames: string[] = [];


}

// auth-role.model.ts
export interface IAuthRole {

  roleName: FormControl<string | null>;
  description: FormControl<string | null>;
  permissionNames: FormControl<string[] | null>;

}

export enum RoleType {
  Normal = 0,
  TenantAutoAdd = 50,
  TenantAdminAdd = 60,

  HiddenFromTenant = 100

}
