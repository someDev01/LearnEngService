namespace Application.Interfaces.Email;

public interface IEmailTemplateRender
{
    string RenderVerificationCode(string code);

    string RenderSignIn();

    string RenderSignUp();
}
