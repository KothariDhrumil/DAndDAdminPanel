using Application.Communication;

namespace Application.Abstractions.Authentication;

public interface ISMSService
{
    Task<string> SendOTPAsync(SMSRequestDTO request);

    Task<string> SendTransactionalSMSASync(SMSRequestDTO request);
}
