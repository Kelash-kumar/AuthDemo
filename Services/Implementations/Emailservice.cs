using AuthDemo.Models;
using AuthDemo.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace AuthDemo.Services.Implementations
{
    public class Emailservice : IEmailService
    {
        private readonly EmailSetting _settings;
        public Emailservice(IOptions<EmailSetting> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(new EmailRequest
            {
                To = to,
                Subject = subject,
                Body = body
            });
        }

        // Full implementation
        public async Task SendEmailAsync(EmailRequest request)
        {
            var email = new MimeMessage();

            // From
            email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.SenderEmail));

            // To
            email.To.Add(MailboxAddress.Parse(request.To));

            // CC (optional)
            foreach (var cc in request.Cc)
                email.Cc.Add(MailboxAddress.Parse(cc));

            // BCC (optional)
            foreach (var bcc in request.Bcc)
                email.Bcc.Add(MailboxAddress.Parse(bcc));

            // Subject
            email.Subject = request.Subject;

            // Body Builder (supports HTML + attachments)
            var builder = new BodyBuilder();

            if (request.IsHtml)
                builder.HtmlBody = request.Body;
            else
                builder.TextBody = request.Body;

            // Attachments (optional)
            foreach (var path in request.AttachmentsPaths)
            {
                if (File.Exists(path))
                    await builder.Attachments.AddAsync(path);
            }

            email.Body = builder.ToMessageBody();

            // Send via SMTP
            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.SmtpHost,
                _settings.SmtpPort,
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
