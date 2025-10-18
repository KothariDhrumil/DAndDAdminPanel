using Application.Identity.Tokens;
using SharedKernel;

namespace Application.Abstractions.Authentication;

public interface ICustomerTokenService
{
    // Verify phone + otp, then return JWT token with “cid”
    Task<Result<TokenResponse>> AuthenticateAsync(CustomerTokenRequest request, CancellationToken cancellationToken);

    // Generate/send OTP for a phone number
    Task<Result> SendOtpAsync(CustomerSendOtpRequest request, CancellationToken cancellationToken);
}

public sealed record CustomerSendOtpRequest(string PhoneNumber);

// Verify request: phone + otp
public sealed record CustomerTokenRequest(string PhoneNumber, string Otp);