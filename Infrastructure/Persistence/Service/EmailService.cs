using Application.Models;
using Application.Interfaces;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, message.From ?? _mailSettings.EmailId));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Content
            };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync(_mailSettings.EmailId, _mailSettings.Password);

                await smtp.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
