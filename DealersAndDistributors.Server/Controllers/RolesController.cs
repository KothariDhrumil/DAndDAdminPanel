
using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using AuthPermissions.BaseCode.PermissionsCode;
using ExamplesCommonCode.CommonAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;
using NSwag.Annotations;
using Shared;
using SharedKernel;

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
    public async Task<IActionResult> GetListAsync()
    {
        string? userId = User.GetUserIdFromUser();
        var data = await _authRolesAdmin.QueryRoleToPermissions(null, currentUserId: userId)
                                        .OrderBy(x => x.RoleType)
                                        .ToListAsync();
        return Ok(PagedResult<List<RoleWithPermissionNamesDto>>.Success(data));
    }

    [HttpGet("permissions")]
    [HasPermission(Permissions.PermissionRead)]
    [OpenApiOperation("Get permissions. This should not be used by a user that has a tenant.", "")]
    public IActionResult ListPermissions()
    {
        var tenantId = User.GetTenantIdFromUser();
        return Ok(PagedResult<List<PermissionDisplay>>.Success(_authRolesAdmin.GetPermissionDisplay(false, tenantId: tenantId)));

    }

    // add get call for getting list of roles by role type 
    [HttpGet("get-roles-by-type")]
    [HasPermission(Permissions.RoleRead)]

    [OpenApiOperation("Get a list of all roles by role type", "")]
    public async Task<IActionResult> GetListByRoleTypeAsync([FromQuery] RoleTypes roleTypes)
    {
        string? userId = User.GetUserIdFromUser();
        var data = await _authRolesAdmin.QueryRoleToPermissions(roleTypes, userId)
                                        .OrderBy(x => x.RoleType)
                                        .ToListAsync();
        return Ok(PagedResult<List<RoleWithPermissionNamesDto>>.Success(data));
    }

    [HttpPut("{id:int}")]
    [HasPermission(Permissions.RoleChange)]
    [OpenApiOperation("Update a role's permission names and optionally it's description. This should not be used by a user that has a tenant.", "")]
    public async Task<IActionResult> Edit(int id, RoleCreateUpdateDto input)
    {
        StatusGeneric.IStatusGeneric status = await _authRolesAdmin
            .UpdateRoleToPermissionsAsync(id, input.RoleName, input.GetSelectedPermissionNames(), input.Description, input.RoleType);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
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
            : Ok(Result.Success(status.Message));
    }

    [HttpGet("{id:int}")]
    [HasPermission(Permissions.RoleChange)]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.GetUserIdFromUser();
        var role = await
            _authRolesAdmin.QueryRoleToPermissions(null, userId).SingleOrDefaultAsync(x => x.RoleId == id);
        var permissionsDisplay = _authRolesAdmin.GetPermissionDisplay(false);

        return Ok(Result.Success(role == null ? null : RoleCreateUpdateDto.SetupForCreateUpdate(role.RoleName, role.Description,
            role.PermissionNames, permissionsDisplay, role.RoleType)));
    }



    [HttpDelete("{id:int}")]
    [HasPermission(Permissions.RoleChange)]
    [OpenApiOperation("Delete a role. This should not be used by a user that has a tenant.", "")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var tenantId = User.GetTenantIdFromUser();

        StatusGeneric.IStatusGeneric status = await _authRolesAdmin.DeleteRoleAsync(id, true, tenantId);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
    }
}