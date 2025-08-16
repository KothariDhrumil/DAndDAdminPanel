
using Application.Abstractions.Authentication;
using Application.Communication;
using Application.Identity.Account;
using Domain;
using Infrastructure.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.TermStore;
using SharedKernel;
using Xunit.Sdk;
using static org.apache.zookeeper.KeeperException;

namespace Infrastructure.Identity;

internal class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ISMSService sMSService;
    public AccountService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        ISMSService sMSService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        this.sMSService = sMSService;
    }

    public async Task<Result<string>> ForgotPassword(string phoneNumber, string origin)
    {
        var account = await _userManager.FindByNameAsync(phoneNumber);

        // always return ok response to prevent email enumeration
        if (account == null)
            return Result.Failure<string>(GenericErrors.UserNotFound);

        string code = await _userManager.GenerateChangePhoneNumberTokenAsync(account, phoneNumber);

        //var code = await _userManager.GenerateTwoFactorTokenAsync(account, _userManager.Options.Tokens.PasswordResetTokenProvider);

        await sMSService.SendOTPAsync(new SMSRequestDTO() { To = phoneNumber, Body = $"{code}", Template = "DELUX_OTP" });

        return code;
    }


    public async Task<Result<string>> ResetPassword(ResetPasswordRequest model)
    {
        var account = await _userManager.FindByNameAsync(model.PhoneNumber);

        // always return ok response to prevent email enumeration
        if (account == null)
            return Result.Failure<string>(GenericErrors.UserNotFound);

        var result = await _userManager.VerifyChangePhoneNumberTokenAsync(account, model.Code,model.PhoneNumber);

        if (result)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(account);
            
            await _userManager.ResetPasswordAsync(account, token, model.Password);

            return account.Id;
        }
        return Result.Failure<string>(GenericErrors.UserNotFound);
    }
}
