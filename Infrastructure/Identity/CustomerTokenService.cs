using Application.Abstractions.Authentication;
using Application.Identity.Tokens;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using AuthPermissions.BaseCode.PermissionsCode;
using Infrastructure.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity;

internal sealed class CustomerTokenService : ICustomerTokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthPermissionsDbContext authContext;
    private readonly JwtSettings _jwtSettings;

    public CustomerTokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        AuthPermissionsDbContext authContext)
    {
        _userManager = userManager;
        this.authContext = authContext;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result> SendOtpAsync(CustomerSendOtpRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return Result.Failure(Error.Validation("PhoneEmpty", "Phone number is required."));

        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, ct);

        // Auto-provision an identity entry on first OTP request (customer user)
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = request.PhoneNumber,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = false,

            };
            var create = await _userManager.CreateAsync(user);
            if (!create.Succeeded)
                return Result.Failure(Error.Problem("UserCreateFailed",
                    string.Join(",", create.Errors.Select(e => e.Description))));
        }

        // Use Identity's phone provider token for OTP
        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

        // TODO: send via SMS provider
        // await _smsService.SendOTPAsync(new SMSRequestDTO { To = request.PhoneNumber, Body = code, Template = "CUSTOMER_LOGIN_OTP" });

        return Result.Success(code);
    }

    public async Task<Result<TokenResponse>> AuthenticateAsync(CustomerTokenRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Otp))
            return Result.Failure<TokenResponse>(Error.Validation("PhoneOrOtpEmpty", "Phone and OTP are required."));

        var user = await _userManager.Users
            .SingleOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, ct);

        if (user is null)
            return Result.Failure<TokenResponse>(Error.Problem("CustomerNotFound", "Customer not found."));

        var ok = await _userManager.VerifyChangePhoneNumberTokenAsync(user, request.Otp, request.PhoneNumber);
        if (!ok)
            return Result.Failure<TokenResponse>(Error.Validation("OtpInvalid", "Invalid OTP."));

        if (!user.PhoneNumberConfirmed)
        {
            user.PhoneNumberConfirmed = true;
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
                return Result.Failure<TokenResponse>(Error.Problem("UserUpdateFailed",
                    string.Join(",", update.Errors.Select(e => e.Description))));
        }

        var customerAccount = await authContext.CustomerAccounts.FirstOrDefaultAsync(x => x.GlobalUserId == user.Id);
        if (customerAccount is null)
            return Result.Failure<TokenResponse>(Error.Problem("CustomerNotFound", "Customer not found."));


        string globalCustomerId = customerAccount.GlobalCustomerId.ToString(); // one user = one account

        var claims = new List<Claim>
        {
            new(PermissionConstants.GlobalCustomerId, globalCustomerId),
            new(ClaimTypes.Name, user.UserName ?? user.PhoneNumber ?? globalCustomerId),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(PermissionConstants.LoggedInUserRole,PermissionConstants.CustomerRole)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.CustomerKey,
            audience: _jwtSettings.CustomerKey,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Result.Success(new TokenResponse(tokenString, string.Empty, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)));
    }
}