using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using AuthPermissions.BaseCode.SetupCode;
using AuthPermissions.SupportCode.AddUsersServices;

namespace Infrastructure.Auth.AuthP;

public static class AppAuthSetupData
{

    public static readonly List<BulkLoadRolesDto> RolesDefinition = new()
    {
           new("SuperAdmin", "Super admin - only use for setup", "AccessAll"),
           new("Tenant Admin", "Tenant-level admin", "AccessAll",RoleTypes.TenantAdminAdd),
    };

    public static readonly List<BulkLoadUserWithRolesTenant> UsersRolesDefinition = new()
    {
        new ("Super@g1.com", null, "SuperAdmin"),
    };
}

