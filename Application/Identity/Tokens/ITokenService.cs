
using Application;
using SharedKernel;

namespace Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<Result> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken);

    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);

    Task<Result<TokenResponse>> ConfirmPhoneAsync(string phoneNumber, string code, string ipAddress);

    Task<Result<string>> GenerateOTPAsync(GenerateOTPRequest request);
}

