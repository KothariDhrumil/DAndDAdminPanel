using Application.Communication;

namespace Application.Abstractions.Authentication;

public interface ISMSService : ITransientService
{
    Task<string> SendOTPAsync(SMSRequestDTO request);

    Task<string> SendTransactionalSMSASync(SMSRequestDTO request);
}
