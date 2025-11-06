using Application.Communication;

namespace Application.Abstractions.Authentication;

public interface IMailService
{
    Task SendAsync(MailRequest request);
}