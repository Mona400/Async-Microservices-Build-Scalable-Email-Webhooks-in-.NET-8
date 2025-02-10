using MailKit.Net.Smtp;
using MimeKit;
using Shared.DTOs;

namespace EmailNotificationWebhooks.Service
{
    public class EmailService : IEmailService
    {
        public string SendEmail(EmailDTO emailDTO)
        {
            //fack email you can generate from this website:https://ethereal.email/create
            var _email =new MimeMessage();
            _email.From.Add(MailboxAddress.Parse("anahi.huel@ethereal.email"));
            _email.To.Add(MailboxAddress.Parse("anahi.huel@ethereal.email"));
            _email.Subject=emailDTO.Title;
            _email.Body= new TextPart(MimeKit.Text.TextFormat.Html) { Text=emailDTO.Content };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("anahi.huel@ethereal.email", "DzYsPaTuuuNwpHvnNm", CancellationToken.None);
            smtp.Send(_email);
            smtp.Disconnect(true);
            return "Email Sent";
        }
    }
}
