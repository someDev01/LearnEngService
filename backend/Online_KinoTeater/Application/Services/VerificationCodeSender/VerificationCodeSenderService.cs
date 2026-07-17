using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.Code;
using Application.Interfaces.Email;
using Application.Interfaces.VerificationCode;
using Application.Interfaces.VerificationCodeSender;
using Domain.Model.Common;

namespace Application.Services.VerificationCodeSender;

public enum VerificationCodePurpose
{
    Authentication,
    ChangeEmail,
}

public class VerificationCodeSenderService(
    IAuthVerificationPolicyService authVerificationPolicyService,
    IVerificationCodeService verificationCodeService,
    ICodeGenerationService codeGeneration,
    IEmailService emailService,
    IEmailTemplateRender emailTemplateRender) : IVerificationCodeSenderService
{
    public async Task<Result<TimeSpan>> SendCodeAsync(
        string email, 
        VerificationCodePurpose purpose, 
        CancellationToken cancellationToken)
    {
        #region CAN SEND CODE
        var canSendCode = await authVerificationPolicyService.CanSendCodeAsync(email);
        if (!canSendCode)
            return Result<TimeSpan>.Failure("Отправка кода пока недоступна");
        #endregion

        #region GENERATION CODE AND BODY
        string code = codeGeneration.GenerationAsync();

        string topic;
        string body;
        
        switch (purpose)
        {
            case VerificationCodePurpose.Authentication:
                topic = "Код подтверждения";
                body = emailTemplateRender.RenderVerificationCode(code);
                break;

            case VerificationCodePurpose.ChangeEmail:
                topic = "Подтверждение смены почты";
                body = emailTemplateRender.RenderVerificationCode(code);
                break;

            default:
                return Result<TimeSpan>.Failure("Неизвестная цель отправки кода");
        }
        #endregion

        #region HASHING CODE AND SET CODE
        await verificationCodeService.SaveCodeAsync(email, code);
        #endregion

        #region LOCK RESEND CODE
        var lockTtl = await authVerificationPolicyService.LockCodeSendingAsync(email);
        #endregion

        #region SENDING
        _ = emailService.SendAsync(email, topic, body, cancellationToken);
        #endregion

        return Result<TimeSpan>.Success(TimeSpan.FromSeconds(lockTtl.TotalSeconds));
    }
}