using System.Net.Http.Json;
using Application.Interfaces.Email;

namespace Infrastructure.Services.Email.Smtp;

public class ResendEmailService(HttpClient client) : IEmailService
{
    public async Task SendAsync(string to, string topic, string body, CancellationToken cancellationToken)
    {
        var response = new
        {
            from = "onboarding@resend.dev",
            to,
            subject = topic,
            html = body
        };

        await client.PostAsJsonAsync("emails", response, cancellationToken);
    }
}
