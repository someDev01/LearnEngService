using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.Code;
using Application.Interfaces.Email;
using Application.Interfaces.VerificationCode;
using Domain.Model.Common;
using FluentValidation;
using MediatR;

namespace Application.Auth.Commands.Send;

public class SendVerificationCodeCommandHandler(
    IAuthVerificationPolicyService authVerificationPolicyService,
    IVerificationCodeService verificationCodeService,
    ICodeGenerationService codeGeneration,
    IEmailService emailService,
    IEmailTemplateRender emailTemplateRender,
    IValidator<SendVerificationCodeCommand> validator) : IRequestHandler<SendVerificationCodeCommand, Result<TimeSpan>>
{
    public async Task<Result<TimeSpan>> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        #region CAN SEND CODE
        var canSendCode = await authVerificationPolicyService.CanSendCodeAsync(request.Email);
        if (!canSendCode)
            return Result<TimeSpan>.Failure("Отправка кода пока недоступна");
        #endregion

        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<TimeSpan>.Failure(errors);
        }
        #endregion

        #region GENERATION CODE AND BODY
        string code = codeGeneration.GenerationAsync();
        string topic = "Код подтверждения";
        string body = emailTemplateRender.RenderVerificationCode(code);
        #endregion

        #region HASHING CODE AND SET CODE
        await verificationCodeService.SaveCodeAsync(request.Email, code);
        #endregion

        #region LOCK RESEND CODE
        var lockTtl = await authVerificationPolicyService.LockCodeSendingAsync(request.Email);
        #endregion

        #region SENDING
        _ = emailService.SendAsync(request.Email, topic, body, cancellationToken);
        #endregion

        return Result<TimeSpan>.Success(TimeSpan.FromSeconds(lockTtl.TotalSeconds));
    }
}
