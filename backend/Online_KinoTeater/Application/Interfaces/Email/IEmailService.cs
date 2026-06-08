namespace Application.Interfaces.Email;

public interface IEmailService
{
    Task SendAsync(string to, string topic, string body, CancellationToken cancellationToken);
}
