using SharedKernel;

namespace Application.Identity.Account;

public interface IAccountService : ITransientService
{
    Task<Result> ForgotPassword(string phoneNumber, string origin);

    Task<Result> ResetPassword(ResetPasswordRequest model);
}

