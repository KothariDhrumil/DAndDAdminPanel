using Application.Communication;

namespace Application.Abstractions.SMS;

public interface IMailService
{
    Task SendAsync(MailRequest request);
}