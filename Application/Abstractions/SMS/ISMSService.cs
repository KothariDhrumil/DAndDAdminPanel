using Application.Communication;

namespace Application.Abstractions.SMS;

public interface ISMSService : ITransientService
{
    Task<string> SendOTPAsync(SMSRequestDTO request);

    Task<string> SendTransactionalSMSASync(SMSRequestDTO request);
}
