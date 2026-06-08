namespace Infrastructure.Settings.Email;

public class EmailSettings
{
    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; }

    public string From { get; set; } = string.Empty;

    public string SmtpUser { get; set; } = string.Empty;

    public string SmtpPass { get; set; } = string.Empty;
}
