using Shared.DTOs;

namespace EmailNotificationWebhooks.Service
{
    public interface IEmailService
    {
        string SendEmail(EmailDTO emailDTO);
    }
}
