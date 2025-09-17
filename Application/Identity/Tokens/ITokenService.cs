
using Application;
using SharedKernel;

namespace Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<Response> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken);

    Task<Response<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);

    Task<Response<TokenResponse>> ConfirmPhoneAsync(string phoneNumber, string code, string ipAddress);

    Task<Response<string>> GenerateOTPAsync(GenerateOTPRequest request);
}

