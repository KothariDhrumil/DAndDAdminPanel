
using Application;
using SharedKernel;

namespace Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<Response<TokenResponse>> AuthenticateAsync(TokenRequest request, CancellationToken cancellationToken);

    Task<Response<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}

