using Application.Contracts;
using Application.Exceptions;
using Application.Models;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> settings,ILogger<EmailService> logger)
        {
            _emailSettings = settings.Value;
            _logger = logger;
        }
        public async Task<bool> SendEmail(Email email)
        {
                var _emailMime = new MimeMessage();
                _emailMime.Sender = new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress);
                _emailMime.To.Add(MailboxAddress.Parse(email.To));
                _emailMime.Subject = email.Subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = email.Body;
                _emailMime.Body = builder.ToMessageBody();
            try
            {
                using var smtp = new SmtpClient();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.Auto);
                smtp.Authenticate(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                await smtp.SendAsync(_emailMime);
                smtp.Disconnect(true);
                _logger.LogInformation("Email sent");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"FromEmail service: {ex.Message}");
                throw new ApiException("Email failed");
            }

        }
    }
}
