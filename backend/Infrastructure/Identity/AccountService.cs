
using Application.Abstractions.Authentication;
using Application.Communication;
using Application.Identity.Account;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Identity;

internal class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ISMSService sMSService;
    public AccountService(
        UserManager<ApplicationUser> userManager,
        ISMSService sMSService)
    {
        _userManager = userManager;
        this.sMSService = sMSService;
    }

    public async Task<Result> ForgotPassword(string phoneNumber, string origin)
    {
        var account = await _userManager.FindByNameAsync(phoneNumber);

        // always return ok response to prevent email enumeration
        if (account == null)
            return Result.Failure(GenericErrors.UserNotFound);

        string code = await _userManager.GenerateChangePhoneNumberTokenAsync(account, phoneNumber);

        //var code = await _userManager.GenerateTwoFactorTokenAsync(account, _userManager.Options.Tokens.PasswordResetTokenProvider);

        await sMSService.SendOTPAsync(new SMSRequestDTO() { To = phoneNumber, Body = $"{code}", Template = "DELUX_OTP" });

        return Result.Success(code);
    }


    public async Task<Result> ResetPassword(ResetPasswordRequest model)
    {
        var account = await _userManager.FindByNameAsync(model.PhoneNumber);

        // always return ok response to prevent email enumeration
        if (account == null)
            return Result.Failure(GenericErrors.UserNotFound);

        var result = await _userManager.VerifyChangePhoneNumberTokenAsync(account, model.Code, model.PhoneNumber);

        if (result)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(account);

            await _userManager.ResetPasswordAsync(account, token, model.Password);

            return Result.Success(account.Id);
        }
        return Result.Failure(GenericErrors.UserNotFound);
    }
}
