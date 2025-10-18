
using Application;
using SharedKernel;

namespace Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<Result> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken);

    Task<Result> RefreshTokenAsync(RefreshTokenRequest request);

    Task<Result> ConfirmPhoneAsync(string phoneNumber, string code, string ipAddress);

    Task<Result> GenerateOTPAsync(GenerateOTPRequest request);
}

