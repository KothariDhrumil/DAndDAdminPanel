
using Application;
using SharedKernel;

namespace Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<Result<TokenResponse>> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken);

    Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}

