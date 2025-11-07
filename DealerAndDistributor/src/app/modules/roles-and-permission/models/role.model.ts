import { RoleTypes } from "./enums/roletypes.enum";

export interface Role {
	roleId: string;
	roleName: string;
	description: string;
	roleType: RoleTypes | string;
	permissionNames: string[];
}
 
