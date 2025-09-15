
using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.PermissionsCode;
using ExamplesCommonCode.CommonAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Shared;

namespace DealersAndDistributors.Server.Controllers;

public class RolesController : VersionNeutralApiController
{
    private readonly IAuthRolesAdminService _authRolesAdmin;

    public RolesController(IAuthRolesAdminService authRolesAdmin)
    {
        _authRolesAdmin = authRolesAdmin;
    }

    [HttpGet]
    [HasPermission(Permissions.RoleRead)]
    [OpenApiOperation("Get a list of all roles.", "")]
    public async Task<PaginatedResult<List<RoleWithPermissionNamesDto>>> GetListAsync()
    {
        string? userId = User.GetUserIdFromUser();
       var data = await _authRolesAdmin.QueryRoleToPermissions(userId)
                                       .OrderBy(x => x.RoleType)
                                       .ToListAsync();
        return new PaginatedResult<List<RoleWithPermissionNamesDto>>(data);
    }

    [HttpGet("permissions")]
    [HasPermission(Permissions.PermissionRead)]
    [OpenApiOperation("Get permissions. This should not be used by a user that has a tenant.", "")]
    public PaginatedResult<List<PermissionDisplay>> ListPermissions()
    {
        return new PaginatedResult<List<PermissionDisplay>>(_authRolesAdmin.GetPermissionDisplay(false));
        
    }

    [HttpPut("permissions")]
    [HasPermission(Permissions.RoleChange)]
    [OpenApiOperation("Update a role's permission names and optionally it's description. This should not be used by a user that has a tenant.", "")]
    public async Task<IActionResult> Edit(RoleCreateUpdateDto input)
    {
        StatusGeneric.IStatusGeneric status = await _authRolesAdmin
            .UpdateRoleToPermissionsAsync(input.RoleName, input.GetSelectedPermissionNames(), input.Description, input.RoleType);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }

    [HttpPost]
    [HasPermission(Permissions.RoleChange)]
    [OpenApiOperation("Create a role. This should not be used by a user that has a tenant.", "")]
    public async Task<ActionResult> RegisterRoleAsync(RoleCreateUpdateDto input)
    {
        var tenantId = User.GetTenantIdFromUser();

        StatusGeneric.IStatusGeneric status = await _authRolesAdmin
                .CreateRoleToPermissionsAsync(input.RoleName, input.GetSelectedPermissionNames(), input.Description, input.RoleType, tenantId);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }

    [HttpDelete]
    [HasPermission(Permissions.RoleChange)]
    [OpenApiOperation("Delete a role. This should not be used by a user that has a tenant.", "")]
    public async Task<ActionResult> DeleteAsync(RoleDeleteConfirmDto input)
    {
        var tenantId = User.GetTenantIdFromUser();
        
        StatusGeneric.IStatusGeneric status = await _authRolesAdmin.DeleteRoleAsync(input.RoleName, input.ConfirmDelete?.Trim() == input.RoleName, tenantId);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }
}