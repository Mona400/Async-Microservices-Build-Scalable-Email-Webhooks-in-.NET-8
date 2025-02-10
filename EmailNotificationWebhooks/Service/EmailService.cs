using MailKit.Net.Smtp;
using MimeKit;
using Shared.DTOs;

namespace EmailNotificationWebhooks.Service
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailDTO emailDTO)
        {
            var _email =new MimeMessage();
            _email.From.Add(MailboxAddress.Parse(""));
            _email.To.Add(MailboxAddress.Parse(""));
            _email.Subject=emailDTO.Title;
            _email.Body= new TextPart(MimeKit.Text.TextFormat.Html) { Text=emailDTO.Content };
            using var smtp = new SmtpClient();
            smtp.Connect("",587,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("", "", CancellationToken.None);
            smtp.Send(_email);
            smtp.Disconnect(true);
            return "Email Sent";
        }
    }
}
