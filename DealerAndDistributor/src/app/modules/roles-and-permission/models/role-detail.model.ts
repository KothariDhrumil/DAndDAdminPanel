export interface RoleDetail {
    roleId?: number;
    roleName: string;
    description?: string;
    roleType: number;
    permissionsWithSelect: PermissionsWithSelect[];
}

export interface PermissionsWithSelect {
    permissionName: string;
    selected: boolean;
    groupName: string;

    description?: string;
}