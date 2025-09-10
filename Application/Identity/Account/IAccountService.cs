using SharedKernel;

namespace Application.Identity.Account;

public interface IAccountService : ITransientService
{
    Task<Response> ForgotPassword(string phoneNumber, string origin);

    Task<Response> ResetPassword(ResetPasswordRequest model);
}

