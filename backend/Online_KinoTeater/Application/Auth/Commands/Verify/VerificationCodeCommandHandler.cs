using Application.Auth.Dtos;
using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.Email;
using Application.Interfaces.Token;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.VerificationCode;
using Application.Settings.Code;
using Domain.Model.Common;
using Domain.Model.Entyties;
using Domain.Model.ValueObjects;
using Domain.Repositories.User;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Auth.Commands.Verify;

public class VerificationCodeCommandHandler(
    IAuthVerificationPolicyService authVerificationPolicyService,
    IVerificationCodeService verificationCodeService,
    IUserRepository userRepository,
    ITokenService tokenService,
    IEmailService emailService,
    IEmailTemplateRender emailTemplateRender,
    IValidator<VerificationCodeCommand> validator,
    IUnitOfWork unitOfWork,
    IOptions<CodeSettings> codeConfig) : IRequestHandler<VerificationCodeCommand, Result<VerifyDto>>
{
    private readonly CodeSettings _codeSettings = codeConfig.Value;

    public async Task<Result<VerifyDto>> Handle(VerificationCodeCommand request, CancellationToken cancellationToken)
    {
        #region ATTEMPTS
        long maxAttempts = _codeSettings.UserMaxAttempts;
        var isAttemptsBlocked = await authVerificationPolicyService.IsVerificationAttemptsBlockedAsync(
            request.Email, 
            maxAttempts);
        if (!isAttemptsBlocked)
            return Result<VerifyDto>.Failure("Лимит на кол-во попыток превышен. Введите код позже");
        #endregion

        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result<VerifyDto>.Failure(errors);
        }
        #endregion

        #region VERIFY
        var userSendCode = await verificationCodeService.VerifyCodeAsync(request.Email, request.Code);
        if (userSendCode is null)
        {
            await authVerificationPolicyService.IncrementVerificationAttemptsAsync(request.Email);
            return Result<VerifyDto>.Failure("Код неправильный!");
        }
        #endregion

        #region CHECK USER
        var userExisting = await userRepository.GetByEmailAsync(userSendCode.Email, cancellationToken);
        bool isNewUser = userExisting is null;
        if (userExisting is null)
        {
            #region VALUEOBJECTS/ONE -> DOMAIN
            var emailResult = Email.Create(userSendCode.Email);
            if (!emailResult.IsSuccess)
                return Result<VerifyDto>.Failure(emailResult.Error!);

            var email = emailResult.Value!;

            var role = Role.User;
            #endregion

            var userResult = User.Create(
                email,
                role);
            if (!userResult.IsSuccess)
                return Result<VerifyDto>.Failure(userResult.Error!);

            var user = userResult.Value!;

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            userExisting = user;
        }

        else
        {
            if (userExisting.Role != Role.User)
                return Result<VerifyDto>.Failure("Ошибка входа, Неверные данные");
        }
        #endregion

        #region JWT
        var jwt = tokenService.GenerationToken(userExisting!);
        if (jwt is null)
            return Result<VerifyDto>.Failure($"Ошибка создания токена");
        #endregion

        #region SEND 
        if(isNewUser)
        {
            string topic = "Успешная регистрация";
            string body = emailTemplateRender.RenderSignUp();
            await emailService.SendAsync(request.Email, topic, body, cancellationToken);
        }
        else
        {
            string topic = "Успешный вход";
            string body = emailTemplateRender.RenderSignIn();
            await emailService.SendAsync(request.Email, topic, body, cancellationToken);
        }
        #endregion

        #region DELETE KEYS
        await verificationCodeService.DeleteCodeAsync(request.Email);
        await authVerificationPolicyService.ResetVerificationAttemptsAsync(request.Email);
        #endregion

        var result = new VerifyDto(jwt, userExisting.Email!.Value);

        return Result<VerifyDto>.Success(result);
    }
}
