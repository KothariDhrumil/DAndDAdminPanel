
using Application.Identity.Account;
using Application.Identity.Tokens;
using AuthPermissions.AspNetCore.Services;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.SupportCode.AddUsersServices;
using DealersAndDistributors.Server.Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedKernel;
using StatusGeneric;
namespace DealersAndDistributors.Server.Controllers;

public sealed class AccountController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;
    private readonly IDisableJwtRefreshToken _disableJwtRefreshService;

    public AccountController(ITokenService tokenService, IDisableJwtRefreshToken disableJwtRefreshService)
    {
        _tokenService = tokenService;
        _disableJwtRefreshService = disableJwtRefreshService;
    }

    [HttpPost("authenticate")]
    [AllowAnonymous]
    // [OpenApiOperation("Request an access token using credentials.", "")]
    public async Task<IActionResult> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _tokenService.AuthenticateAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    //[OpenApiOperation("Request an access token using a refresh token.", "")]
    public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request)
    {
        var response = await _tokenService.RefreshTokenAsync(request);
        return Ok(response);

    }

    [HttpPost("register")]
    [AllowAnonymous]
    [OpenApiOperation("Creates a new user and tenant with roles.", "")]
    public async Task<ActionResult> CreateUserAndTenantAsync([FromServices] ISignInAndCreateTenant userRegisterInvite, CreateUserRequest request)
    {
        var newUserData = new AddNewUserDto
        {
            Email = $"{request.PhoneNumber}@dealers.com",
            UserName = request.UserName,
            Password = request.Password,
            IsPersistent = false,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DesignationId = request.DesignationId,
            Roles = new List<string> { "Feature - Stock", "Feature - Sales" },
        };
        var newTenantData = new AddNewTenantDto
        {
            TenantName = request.TenantName,
            HasOwnDb = false,

        };
        var status = await userRegisterInvite.SignUpNewTenantAsync(newUserData, newTenantData);
        if (status.HasErrors)
        {
            return Ok(SharedKernel.Response.Failure<string>(new Error("101", status.GetAllErrors(","), ErrorType.Failure)));
        }
        return Ok(SharedKernel.Response.Success<string>(status.Message));
    }

    [HttpGet("generate-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateOTPAsync(GenerateOTPRequest request)
    {
        var origin = Request.Headers.Origin;
        return Ok(await _tokenService.GenerateOTPAsync(request));
    }

    [HttpGet("confirm-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmOTPAsync([FromQuery] string PhoneNumber, [FromQuery] string code)
    {
        var origin = Request.Headers.Origin;
        return Ok(await _tokenService.ConfirmPhoneAsync(PhoneNumber, code, GenerateIPAddress()));
    }

    private string GenerateIPAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }


    /// <summary>
    /// This will mark the JST refresh as used, so the user cannot refresh the JWT Token.
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    // [OpenApiOperation("Mark the access token as so the user cannot refresh the token.", "")]
    public async Task<IResult> Logout()
    {
        if (User.GetUserIdFromUser() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }
        await _disableJwtRefreshService.LogoutUserViaUserIdAsync(userId);
        return Results.Ok();

    }

    /*private string GetIpAddress() =>
        Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"]
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    */
}