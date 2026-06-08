using Microsoft.Extensions.Options;
using Application.Interfaces.Email;
using Infrastructure.Settings.Email;
using MimeKit;
using MailKit.Net.Smtp;

namespace Infrastructure.Services.Email.Smtp;

public class SmtpEmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(string to, string topic, string body, CancellationToken cancellationToken)
    {
        string smtpHost = _settings.SmtpHost ?? "";
        int smtpPort = _settings.SmtpPort;
        string fromEmail = _settings.From ?? "";
        string smtpUser = _settings.SmtpUser ?? "";
        string stmpPass = _settings.SmtpPass ?? "";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("VoClip", $"{fromEmail}"));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = topic;

        message.Body = new TextPart("html")
        {
            Text = body,
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync(smtpUser, stmpPass, cancellationToken);
        await client.SendAsync(message, cancellationToken);

        await client.DisconnectAsync(true, cancellationToken);
    }
}
