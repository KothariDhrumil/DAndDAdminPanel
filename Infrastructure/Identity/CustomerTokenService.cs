using Application.Abstractions.Authentication;
using Application.Communication;
using Application.Identity.Tokens;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Infrastructure.Auth.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Net.DistributedFileStoreCache;
using SharedKernel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity;

internal sealed class CustomerTokenService : ICustomerTokenService
{
    private readonly AuthPermissionsDbContext _authDb;
    private readonly JwtSettings _jwtSettings;
    private readonly IDistributedFileStoreCacheClass _fsCache;
    private readonly ISMSService _smsService;

    private const string OtpCachePrefix = "CustomerOtp-";
    private const int OtpExpiryMinutes = 5;

    public CustomerTokenService(
        AuthPermissionsDbContext authDb,
        IOptions<JwtSettings> jwtSettings,
        IDistributedFileStoreCacheClass fsCache,
        ISMSService smsService)
    {
        _authDb = authDb;
        _jwtSettings = jwtSettings.Value;
        _fsCache = fsCache;
        _smsService = smsService;
    }

    public async Task<Result> SendOtpAsync(CustomerSendOtpRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return Result.Failure(Error.Failure("PhoneEmpty", "Phone number is required."));

        var customer = await _authDb.CustomerAccounts
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber && c.IsActive, cancellationToken);

        if (customer is null)
            return Result.Failure(Error.Problem("CustomerNotFound", "Customer not found or inactive."));

        string code = GenerateSixDigitCode();
        var payload = new OtpPayload
        {
            Code = code,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes),
        };

        _fsCache.SetClass(OtpCachePrefix + request.PhoneNumber, payload);

        // Optionally send SMS (uncomment if you have SMS provider wired)
        try
        {
            await _smsService.SendOTPAsync(new SMSRequestDTO
            {
                To = request.PhoneNumber,
                Body = code,
                Template = "CUSTOMER_LOGIN_OTP"
            });
        }
        catch
        {
            // Swallow errors to avoid leaking SMS provider issues to callers.
            // OTP is still stored and can be returned via an alternate channel if needed.
        }

        return Result.Success();
    }

    public async Task<Result<TokenResponse>> AuthenticateAsync(CustomerTokenRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Otp))
            return Result.Failure<TokenResponse>(Error.Failure("PhoneOrOtpEmpty", "Phone and OTP are required."));

        var customer = await _authDb.CustomerAccounts
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber && c.IsActive, cancellationToken);

        if (customer is null)
            return Result.Failure<TokenResponse>(Error.Problem("CustomerNotFound", "Customer not found or inactive."));

        var cacheKey = OtpCachePrefix + request.PhoneNumber;
        var payload = _fsCache.GetClass<OtpPayload>(cacheKey);

        if (payload == null)
            return Result.Failure<TokenResponse>(Error.Problem("OtpNotRequested", "OTP not requested or already used."));

        if (payload.ExpiresUtc < DateTime.UtcNow)
        {
            _fsCache.Remove(cacheKey);
            return Result.Failure<TokenResponse>(Error.Problem("OtpExpired", "OTP has expired. Please request a new OTP."));
        }

        if (!string.Equals(payload.Code, request.Otp, StringComparison.Ordinal))
            return Result.Failure<TokenResponse>(Error.Failure("OtpInvalid", "Invalid OTP."));

        // Invalidate OTP after successful verification
        _fsCache.Remove(cacheKey);

        // Build JWT with ‘cid’ claim
        var claims = new List<Claim>
        {
            new("cid", customer.Id.ToString()),
            new(ClaimTypes.Name, customer.DisplayName ?? request.PhoneNumber),
            new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new("role", "Customer")
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

        // No refresh in this minimal flow
        return Result.Success(new TokenResponse(tokenString, string.Empty, DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)));
    }

    private static string GenerateSixDigitCode()
    {
        // Cryptographically strong 6-digit code
        Span<byte> bytes = stackalloc byte[4];
        RandomNumberGenerator.Fill(bytes);
        var value = BitConverter.ToUInt32(bytes);
        return (value % 1_000_000).ToString("D6");
    }

    private sealed class OtpPayload
    {
        public string Code { get; set; } = default!;
        public DateTime ExpiresUtc { get; set; }
    }
}