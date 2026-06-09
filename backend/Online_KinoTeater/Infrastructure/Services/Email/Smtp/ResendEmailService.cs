using System.Net.Http.Json;
using Application.Interfaces.Email;
using Infrastructure.Settings.Email;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Email.Smtp;

public class ResendEmailService(
    HttpClient client,
    IOptions<EmailSettings> options) : IEmailService
{
    public async Task SendAsync(string to, string topic, string body, CancellationToken cancellationToken)
    {
        var response = new
        {
            from = options.Value.From,
            to,
            subject = topic,
            html = body
        };

        await client.PostAsJsonAsync("emails", response, cancellationToken);
    }
}
