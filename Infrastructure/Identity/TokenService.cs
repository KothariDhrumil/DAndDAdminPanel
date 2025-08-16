using Application.Identity.Tokens;
using AuthPermissions.AspNetCore.JwtTokenCode;
using Domain;
using Infrastructure.Auth.Jwt;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedKernel;

namespace Infrastructure.Identity;
internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenBuilder _tokenBuilder;
    private readonly JwtSettings _jwtSettings;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        ITokenBuilder tokenBuilder,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _tokenBuilder = tokenBuilder;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<TokenResponse>> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new HttpRequestException("Authentication Failed.", null, System.Net.HttpStatusCode.Unauthorized);
        }

        TokenAndRefreshToken result = await _tokenBuilder.GenerateTokenAndRefreshTokenAsync(user.Id);
        return new TokenResponse(result.Token, result.RefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays));
    }

    public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        (var updatedTokens, int _) = await _tokenBuilder.RefreshTokenUsingRefreshTokenAsync(request.Adapt<TokenAndRefreshToken>());
        if (updatedTokens == null)
            throw new HttpRequestException("Refresh Authentication Token Failed.", null, System.Net.HttpStatusCode.Unauthorized);

        return new TokenResponse(updatedTokens.Token, updatedTokens.RefreshToken, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)); ;
    }
}