using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Application.Interfaces.Email;
using Infrastructure.Settings.Email;

namespace Infrastructure.Services.Email.Smtp;

public class SmtpEmailService(IOptions<EmailSettings> options): IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(string to, string topic, string body, CancellationToken cancellationToken)
    {
        string smtpHost = _settings.SmtpHost ?? "";
        int smtpPort = _settings.SmtpPort;
        string fromEmail = _settings.From ?? "";
        string smtpUser = _settings.SmtpUser ?? "";
        string stmpPass = _settings.SmtpPass ?? "";

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpUser, stmpPass)
        };

        using var message = new MailMessage(fromEmail, to, topic, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}
