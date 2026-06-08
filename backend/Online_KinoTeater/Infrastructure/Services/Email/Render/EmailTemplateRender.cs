using Application.Interfaces.Email;
using Application.Settings.Code;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Email.Render;

public class EmailTemplateRender(
    IOptions<CodeSettings> options): IEmailTemplateRender
{
    private readonly CodeSettings _settings = options.Value;

    public string RenderVerificationCode(string code)
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Services",
            "Email",
            "TemplateHtml",
            "Verification.html");
        string html = File.ReadAllText(path);
        string result = html
                .Replace("{{CODE}}", code.ToString())
                .Replace("{{TIME}}", (_settings.ExpireSecondsCode / 60).ToString());
        return result;
    }

    public string RenderSignIn()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Services",
            "Email",
            "TemplateHtml",
            "SignIn.html");
        string html = File.ReadAllText(path);

        return html;
    }

    public string RenderSignUp()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Services",
            "Email",
            "TemplateHtml",
            "SignUp.html");
        string html = File.ReadAllText(path);

        return html;
    }
}
