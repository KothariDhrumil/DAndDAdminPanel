using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.SupportCode.AddUsersServices;
using AuthPermissions.SupportCode.AddUsersServices.Authentication;
using Domain;
using ExamplesCommonCode.CommonAdmin;
using Infrastructure.Auth.AuthP;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using Shared;
using SharedKernel;
using System.Security.Claims;

namespace DealersAndDistributors.Server.Controllers;

/// <summary>
/// Controller for managing authentication users and their associated operations
/// </summary>
public class AuthUsersController : VersionNeutralApiController
{
    private readonly IAuthUsersAdminService _authUsersAdmin;
    private readonly IAddNewUserManager _addNewUserManager;

    /// <summary>
    /// Initializes a new instance of the AuthUsersController
    /// </summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager</param>
    /// <param name="authUsersAdmin">The service for managing authentication users</param>
    /// <param name="addNewUserManager"></param>
    public AuthUsersController(
        UserManager<ApplicationUser> userManager,
        IAuthUsersAdminService authUsersAdmin,
        IAddNewUserManager addNewUserManager)
    {
        _authUsersAdmin = authUsersAdmin;
        this._addNewUserManager = addNewUserManager;
    }

    [HttpGet("listusers")]
    //[HasPermission(Permissions.AccessAll)]
    [OpenApiOperation("List users filtered by authUser tenant.", "")]
    public async Task<IActionResult> ListAuthUsersAsync(int pageNumber, int pageSize, string orderBy)
    {
        string? authDataKey = User.GetAuthDataKeyFromUser();
        IQueryable<AuthUser> userQuery = _authUsersAdmin.QueryAuthUsers(authDataKey);
        var users = await AuthUserDisplay.TurnIntoDisplayFormat(userQuery.OrderBy(x => x.UserTenant.TenantFullName)).ToListAsync();

        return Ok(PagedResult<List<AuthUserDisplay>>.Success(users));
    }
    
    [HttpGet("listusers/{tenantId:int}")]
    //[HasPermission(Permissions.UserRead)]
    [OpenApiOperation("List users filtered by authUser tenant.", "")]
    public async Task<IActionResult> ListAuthUsersByTenantIdAsync(int pageNumber, int pageSize, string orderBy, int tenantId)
    {
        string? authDataKey = User.GetAuthDataKeyFromUser();
        IQueryable<AuthUser> userQuery = _authUsersAdmin.QueryAuthUsers(tenantId);
        var users = await AuthUserDisplay.TurnIntoDisplayFormat(userQuery.OrderBy(x => x.UserTenant.TenantFullName)).ToListAsync();

        return Ok(PagedResult<List<AuthUserDisplay>>.Success(users));
    }

    //[HttpGet("view-sync-changes")]
    //[HasPermission(Permissions.UserSync)]
    ////hide from swagger as this is an internal only method
    //[OpenApiIgnore]
    //public async Task<PaginatedResult<List<SyncAuthUserWithChange>>> SyncUsers()
    //{
    //    var data = await _authUsersAdmin.SyncAndShowChangesAsync();
    //    return new PaginatedResult<List<SyncAuthUserWithChange>>(data);
    //}

    //[HttpPost("apply-sync-changes")]
    //[HasPermission(Permissions.UserSync)]
    //[OpenApiIgnore]
    //public async Task<ActionResult> SyncUsers(IEnumerable<SyncAuthUserWithChange> data)
    //{
    //    var status = await _authUsersAdmin.ApplySyncChangesAsync(data);
    //    if (status.HasErrors)
    //        throw new Exception(status.GetAllErrors());

    //    return Ok(status.Message);
    //}


    // Lets add end point for Adding auth user in tenant , call   ; for this

    [HttpPost]
    //[HasPermission(Permissions.UserRead)]
    [OpenApiOperation("Add User in Tenant")]
    public async Task<ActionResult> CreateAsync(AddNewUserDto newUser)
    {
        var tenantId= User.GetTenantIdFromUser();
        if (newUser.TenantId == null)
        {
            newUser.TenantId = tenantId;
        }

        var status = await _addNewUserManager.SetUserInfoAsync(newUser);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
    }



    [HttpPut]
    //[HasPermission(Permissions.UserChange)]
    [OpenApiOperation("Update an authUser.", "")]
    public async Task<ActionResult> UpdateAsync(SetupManualUserChange change)
    {
        StatusGeneric.IStatusGeneric status = await _authUsersAdmin.UpdateUserAsync(
            change.UserId, change.Email, change.UserName, change.RoleIds, change.TenantName);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
    }

    // todo Change the input type to represent only required changes
    [HttpPut("roles")]
    //[HasPermission(Permissions.UserRolesChange)]
    [OpenApiOperation("Update an authUser's roles.", "")]
    public async Task<ActionResult> UpdateRolesAsync(SetupManualUserChange change)
    {
        StatusGeneric.IStatusGeneric status = await _authUsersAdmin.UpdateUserAsync(
            change.UserId, roleIds: change.RoleIds);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
    }

    [HttpDelete("{id}")]
    //[HasPermission(Permissions.UserRemove)]
    [OpenApiOperation("Delete an authUser.", "")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var status = await _authUsersAdmin.DeleteUserAsync(id);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(Result.Success(status.Message));
    }
}
