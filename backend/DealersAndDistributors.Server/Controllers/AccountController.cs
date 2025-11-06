using Application.Abstractions.Authentication;
using Application.Domain.TeantUser.Services;
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

namespace DealersAndDistributors.Server.Controllers;

public sealed class AccountController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;
    private readonly IDisableJwtRefreshToken _disableJwtRefreshService;
    private readonly ITenantUserOnboardingService tenantUserOnboardingService;

    public AccountController(ITokenService tokenService, 
        IDisableJwtRefreshToken disableJwtRefreshService,
        ITenantUserOnboardingService tenantUserOnboardingService)
    {
        _tokenService = tokenService;
        _disableJwtRefreshService = disableJwtRefreshService;
        this.tenantUserOnboardingService = tenantUserOnboardingService;
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
            Email = $"{request.PhoneNumber}@DandD.com",
            UserName = request.PhoneNumber,
            Password = $"{request.PhoneNumber}@DandD",
            IsPersistent = false,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
        var newTenantData = new AddNewTenantDto
        {
            TenantName = request.TenantName,
            HasOwnDb = request.HasOwnDb,
            ShardingName = request.ShardingName

        };
        var status = await userRegisterInvite.SignUpNewTenantAsync(newUserData, newTenantData);
        if (status.HasErrors)
        {
            return Ok(SharedKernel.Result.Failure<string>(new Error("101", status.GetAllErrors(","), ErrorType.Failure)));
        }
        
        return Ok(SharedKernel.Result.Success<string>(status.Message));
    }
    [HttpGet("generate-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateOTPAsync([FromQuery] GenerateOTPRequest request)
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

    // CUSTOMER: request OTP
    [HttpPost("customer/generate-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> CustomerGenerateOtpAsync(
        [FromServices] ICustomerTokenService customerTokenService,
        [FromBody] CustomerSendOtpRequest request,
        CancellationToken cancellationToken)
    {
        var response = await customerTokenService.SendOtpAsync(request, cancellationToken);
        return Ok(response);
    }

    // CUSTOMER: verify OTP and get JWT
    [HttpPost("customer/authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> CustomerAuthenticateAsync(
        [FromServices] ICustomerTokenService customerTokenService,
        [FromBody] CustomerTokenRequest request,
        CancellationToken cancellationToken)
    {
        var response = await customerTokenService.AuthenticateAsync(request, cancellationToken);
        return Ok(response);
    }
}