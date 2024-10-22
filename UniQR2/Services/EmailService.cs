﻿using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using UniQR2.Models.Configurations;

namespace UniQR2.Services
{
    public class EmailService : IEmailService
    {
        private readonly SMTPSettings _settings;

        public EmailService(IOptions<SMTPSettings> settings)
        {
            this._settings = settings.Value;
        }

        public async Task Send(string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.SmtpUser, _settings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}

