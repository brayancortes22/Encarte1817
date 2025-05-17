using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp; // Cambiado para usar MailKit.Net.Smtp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Utilities.Interfaces;

namespace Utilities.Mail
{
    public class EmailService : IEmailService // Implementando la interfaz
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptionsMonitor<SmtpSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        public async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken = default)
        {
            using var client = new SmtpClient(); // Este es el SmtpClient de MailKit
            await client.ConnectAsync(_settings.Server, _settings.Port ?? 25, _settings.EnableSsl, cancellationToken);

            if (!string.IsNullOrEmpty(_settings.Username))
                await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }

        // Implementación de los métodos requeridos por la interfaz IEmailService
        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailToManyAsync(List<string> to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

                foreach (var recipient in to)
                {
                    message.To.Add(MailboxAddress.Parse(recipient));
                }

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailWithCopyAsync(string to, string subject, string body, List<string> cc = null, List<string> bcc = null, bool isHtml = true)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(to));

                if (cc != null && cc.Count > 0)
                {
                    foreach (var recipient in cc)
                    {
                        message.Cc.Add(MailboxAddress.Parse(recipient));
                    }
                }

                if (bcc != null && bcc.Count > 0)
                {
                    foreach (var recipient in bcc)
                    {
                        message.Bcc.Add(MailboxAddress.Parse(recipient));
                    }
                }

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}