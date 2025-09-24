using Application.Abstractions.Authentication;
using Application.Communication;
using Application.Identity.Tokens;
using AuthPermissions.AspNetCore.JwtTokenCode;
using Domain;
using Infrastructure.Auth.Jwt;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Identity;
internal class TokenService(
    UserManager<ApplicationUser> userManager,
    ITokenBuilder tokenBuilder,
    IOptions<JwtSettings> jwtSettings,
    ISMSService sMSService) : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ITokenBuilder _tokenBuilder = tokenBuilder;
    private readonly ISMSService sMSService = sMSService;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<Result> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x =>
        (!string.IsNullOrEmpty(request.PhoneNumber) && x.PhoneNumber == request.PhoneNumber.Trim())
        || (!string.IsNullOrEmpty(request.Email) && x.Email == request.Email));
        if (user is not null)
        {
            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new HttpRequestException("Authentication Failed.", null, System.Net.HttpStatusCode.Unauthorized);
            }
            //if (request.OtpEnabled)
            //{
            //    string code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            //    if (code != null) 
            //    {
            //        //var code = await _userManager.GenerateTwoFactorTokenAsync(account, _userManager.Options.Tokens.PasswordResetTokenProvider);
            //        await sMSService.SendOTPAsync(new SMSRequestDTO() { To = request.PhoneNumber, Body = $"{code}", Template = "DELUX_OTP" });
            //        return Response.Success();
            //    }
            //}
            TokenAndRefreshToken result = await _tokenBuilder.GenerateTokenAndRefreshTokenAsync(user.Id);
            // TODO: If this is a customer login flow, ensure the 'cid' claim (GlobalCustomerId) is added when building the token
            return Result.Success(new TokenResponse(result.Token, result.RefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)));
        }
        throw new HttpRequestException("Authentication Failed.", null, System.Net.HttpStatusCode.Unauthorized);
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        (var updatedTokens, int _) = await _tokenBuilder.RefreshTokenUsingRefreshTokenAsync(request.Adapt<TokenAndRefreshToken>());
        if (updatedTokens == null)
            throw new HttpRequestException("Refresh Authentication Token Failed.", null, System.Net.HttpStatusCode.Unauthorized);

        return Result.Success(new TokenResponse(updatedTokens.Token, updatedTokens.RefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays))); ;
    }

    public async Task<Result<TokenResponse>> ConfirmPhoneAsync(string phoneNumber, string code, string ipAddress)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        if (user is not null)
        {
            var phoneVerified = await _userManager.VerifyChangePhoneNumberTokenAsync(user, code, phoneNumber);
            if (phoneVerified)
            {
                if (!user.PhoneNumberConfirmed)
                {
                    user.PhoneNumberConfirmed = phoneVerified;
                    await _userManager.UpdateAsync(user);
                }
                TokenAndRefreshToken result = await _tokenBuilder.GenerateTokenAndRefreshTokenAsync(user.Id);
                return Result.Success(new TokenResponse(result.Token, result.RefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)));
            }
            throw new HttpRequestException("Authentication Failed.", null, System.Net.HttpStatusCode.Unauthorized);
        }
        throw new HttpRequestException("Authentication Failed.", null, System.Net.HttpStatusCode.Unauthorized);
    }

    public async Task<Result<string>> GenerateOTPAsync(GenerateOTPRequest request)
    {
        var account = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber) ?? throw new Exception($"{request.PhoneNumber} not registered");

        string code = await _userManager.GenerateChangePhoneNumberTokenAsync(account, account.PhoneNumber);

        //await sMSService.SendOTPAsync(new SMSRequestDTO() { To = account.PhoneNumber, Body = $"{code}", Template = "DELUX_OTP" });

        return Result.Success(code);
    }
}