using Application.Domain.TeantUser.Models;
using Application.Domain.TeantUser.Services;
using Application.Domain.TeantUser.Update;
using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using AuthPermissions.SupportCode.AddUsersServices;
using AuthPermissions.SupportCode.AddUsersServices.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using SharedKernel;
using SharedKernel.CommonAdmin;

namespace DealersAndDistributors.Server.Controllers;

/// <summary>
/// Controller for managing authentication users and their associated operations
/// </summary>
public class AuthUsersController : VersionNeutralApiController
{
    private readonly IAuthUsersAdminService _authUsersAdmin;
    private readonly IAddNewUserManager _addNewUserManager;
    private readonly ITenantUserOnboardingService tenantUserOnboardingService;

    /// <summary>
    /// Initializes a new instance of the AuthUsersController
    /// </summary>
    /// <param name="userManager">The ASP.NET Core Identity user manager</param>
    /// <param name="authUsersAdmin">The service for managing authentication users</param>
    /// <param name="addNewUserManager"></param>
    /// <param name="tenantUserOnboardingService"></param>
    public AuthUsersController(
        UserManager<ApplicationUser> userManager,
        IAuthUsersAdminService authUsersAdmin,
        IAddNewUserManager addNewUserManager,
        ITenantUserOnboardingService tenantUserOnboardingService)
    {
        _authUsersAdmin = authUsersAdmin;
        this._addNewUserManager = addNewUserManager;
        this.tenantUserOnboardingService = tenantUserOnboardingService;
    }

    [HttpGet("listusers/{userTypeId:int}")]
    //[HasPermission(Permissions.AccessAll)]
    [OpenApiOperation("List users filtered by authUser tenant.", "")]
    public async Task<IActionResult> ListAuthUsersAsync(int pageNumber, int pageSize, string orderBy, int userTypeId)
    {
        string? authDataKey = User.GetAuthDataKeyFromUser();
        IQueryable<AuthUser> userQuery = _authUsersAdmin.QueryAuthUsers(authDataKey);
        if (userTypeId > 0)
        {
            var profiles = await tenantUserOnboardingService.GetProfilesByRoleTypeId(userTypeId);
            if (profiles.Count == 0)
            {
                return Ok(PagedResult<List<TenantUserProfileResponse>>.Success());
            }
            var authUsers = userQuery
                                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                                .Where(u => profiles.Select(x => x.UserId).Contains(u.UserId))
                                .AsNoTracking()
                                .ToList();

            // merge the profiles with userQuery to filter users by userTypeId
            var mergedQuery = from user in authUsers
                              join profile in profiles
                              on user.UserId equals profile.UserId
                              select new TenantUserProfileResponse()
                              {
                                  UserId = profile.UserId,
                                  UserType = profile.UserType,
                                  UserTypeId = profile.UserTypeId,
                                  FirstName = user.FirstName,
                                  LastName = user.LastName,
                                  PhoneNumber = user.PhoneNumber,
                                  RoleNames = user.UserRoles.Where(x => x.Role.RoleType != RoleTypes.FeatureRole).Select(y => y.Role.RoleName).ToArray()

                              };
            return Ok(PagedResult<List<TenantUserProfileResponse>>.Success(mergedQuery.ToList()));

        }
        var users = await AuthUserDisplay.TurnIntoDisplayFormat(userQuery.OrderBy(x => x.FirstName)).ToListAsync();

        return Ok(PagedResult<List<AuthUserDisplay>>.Success(users));
    }

    [HttpGet("list-auth-users/{tenantId:int}")]
    //[HasPermission(Permissions.UserRead)]
    [OpenApiOperation("List users filtered by authUser tenant.", "")]
    public async Task<IActionResult> ListAuthUsersByTenantIdAsync(int pageNumber, int pageSize, string orderBy, int tenantId)
    {
        string? authDataKey = User.GetAuthDataKeyFromUser();
        IQueryable<AuthUser> userQuery = _authUsersAdmin.QueryAuthUsers(tenantId);
        var users = await AuthUserDisplay.TurnIntoDisplayFormat(userQuery.OrderBy(x => x.UserTenant.TenantFullName)).ToListAsync();
        return Ok(PagedResult<List<AuthUserDisplay>>.Success(users));
    }

    [HttpPost]
    //[HasPermission(Permissions.UserRead)]
    [OpenApiOperation("Add User in Tenant")]
    public async Task<ActionResult> CreateAsync(AddNewUserDto newUser)
    {
        var tenantId = User.GetTenantId();
        if (newUser.TenantId == null)
        {
            newUser.TenantId = tenantId;
        }
        // TODO : email id patching, remove it later on
        if (string.IsNullOrEmpty(newUser.Email))
        {
            newUser.Email = $"{newUser.PhoneNumber}@dandd.com";
            newUser.Password = $"{newUser.PhoneNumber}@DandD";
        }

        var status = await _addNewUserManager.SetUserInfoAsync(newUser);

        if (status.HasErrors)
            throw new Exception(status.GetAllErrors());

        if (status.Result.TenantId != null)
        {
            await tenantUserOnboardingService.CreateTenantUserProfileIfMissingAsync(
                status.Result.UserId, (int)status.Result.TenantId, newUser.FirstName, newUser.LastName, newUser.PhoneNumber, newUser.UserTypeId, CancellationToken.None);
        }
        return Ok(Result.Success(status.Message));

    }

    [HttpPut]
    //[HasPermission(Permissions.UserChange)]
    [OpenApiOperation("Update an authUser.", "")]
    public async Task<ActionResult> UpdateAsync(SetupManualUserChange change)
    {

        StatusGeneric.IStatusGeneric status = await _authUsersAdmin.UpdateUserAsync(
            change.UserId, change.Email, change.UserName, change.RoleIds, change.TenantName, change.FirstName, change.LastName);

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

    // update method to tenant user in tenant
    [HttpPut("update-tenant-user")]
    [OpenApiOperation("Update a tenant user.", "")]
    public async Task<ActionResult> UpdateTenantUserAsync(UpdateTenantUserModel tenantUser, CancellationToken ct)
    {
        var status = await _addNewUserManager.UpdateUserNameAsync(tenantUser.UserId.ToString(), tenantUser.FirstName, tenantUser.LastName);
        if (status.HasErrors)
            return BadRequest(status.GetAllErrors());

        // Also update the tenant user profile
        await tenantUserOnboardingService.UpdateTenantUserProfileAsync(tenantUser, ct);

        return Ok(Result.Success(status.Message));
    }


}
