using Application.Identity.Tokens;
using Application.Identity.User;
using AuthPermissions.AdminCode;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.PermissionsCode;
using ExamplesCommonCode.CommonAdmin;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers
{
    public class UserController : VersionNeutralApiController
    {
        [HttpGet("UserInfo")]
        public async Task<IActionResult> UserInfo([FromServices] IAuthUsersAdminService service, [FromServices] IUserService userService)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.GetUserIdFromUser();
                if (userId == null)
                {
                    throw new Exception("User Not Found");
                }
                var status = await service.FindAuthUserByUserIdAsync(userId);
                var user = await userService.GetUserDetailsAsync(userId);
                return Ok(new Response<UserInfo>(new UserInfo()
                {
                    AuthUser = !status.HasErrors ? AuthUserDisplay.DisplayUserInfo(status.Result) : null,
                    User = user
                }));
            }
            return null;
        }

        [HttpGet("permissions")]
        public List<string> UserPermissions([FromServices] IUsersPermissionsService service)
        {
            return service.PermissionsFromUser(HttpContext.User);
        }
    }
}