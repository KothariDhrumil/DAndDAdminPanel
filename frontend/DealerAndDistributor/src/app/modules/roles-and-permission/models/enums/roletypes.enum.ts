/**
 * Role types enum defining the different types of roles in the system
 */
export enum RoleTypes {
    /**
     * A Role that can be assigned to any any user
     */
    Normal = 0,

    /**
     * A Role that is assigned to an Tenant and is automatically included in the calculation of the user's Permissions
     */
    TenantAutoAdd = 50,

    /**
     * A Role that is assigned to an Tenant which an admin can assign to a user's list of Roles
     */
    TenantAdminAdd = 60,

    /**
     * A role that is created by Tenant Admin
     */
    TenantCreated = 70,

    /**
     * A Role that is assigned to a Feature, which is then assigned to a Tenant
     */
    FeatureRole = 80,

    /**
     * This Role is hidden from any AuthP user than is linked to a Tenant
     */
    HiddenFromTenant = 100
}
