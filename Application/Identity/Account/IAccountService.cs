using Azure;
using SharedKernel;

namespace Application.Identity.Account;

public interface IAccountService : ITransientService
{
    Task<Result<string>> ForgotPassword(string phoneNumber, string origin);

    Task<Result<string>> ResetPassword(ResetPasswordRequest model);
}

