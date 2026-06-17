using Application.Common.Cache;
using Application.Configs.Admin;
using Application.Interfaces.AuthVerifucationPolicy;
using Application.Interfaces.RoleAdmin;
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
using System.Data;

namespace Application.Auth.Commands.VerifyAdmin;

public class VerificationCodeAndSecretAdminCommandHandler(
    IAuthVerificationPolicyService authVerificationPolicyService,
    IVerificationCodeService verificationCodeService,
    ITokenService tokenService,
    IUserRepository userRepository,
    IAdminService adminService,
    IValidator<VerificationCodeAndSecretAdminCommand> validator,
    IUnitOfWork unitOfWork, 
    IOptions<CodeSettings> codeConfig,
    IOptions<AdminSettings> adminConfig) : IRequestHandler<VerificationCodeAndSecretAdminCommand, Result<string>>
{
    private readonly CodeSettings _codeSettings = codeConfig.Value;
    private readonly AdminSettings _adminSettings = adminConfig.Value;

    public async Task<Result<string>> Handle(VerificationCodeAndSecretAdminCommand request, CancellationToken cancellationToken)
    {
        #region ATTEMPTS
        long maxAttempts = _codeSettings.AdminMaxAttempts;
        var isAttemptsBlocked = await authVerificationPolicyService.IsVerificationAttemptsBlockedAsync(
            request.Email,
            maxAttempts);
        if (!isAttemptsBlocked)
            return Result<string>.Failure("Лимит на кол-во попыток превышен. Введите код позже");
        #endregion

        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result<string>.Failure(errors);
        }
        #endregion

        #region KEYS
        var keyAttempts = CacheKeyBuilder.BuildAttemptsKey(request.Email);
        var ttlKeyAttempts = TimeSpan.FromSeconds(_codeSettings.AttemptsExpireHours);

        var keyCode = CacheKeyBuilder.BuildCodeKey(request.Email);
        TimeSpan ttlKeyCode = TimeSpan.FromSeconds(_codeSettings.ExpireSecondsCode);

        var keyResend = CacheKeyBuilder.BuildResendKey(request.Email);
        #endregion

        #region VERIFY
        var userSendCode = await verificationCodeService.VerifyCodeAsync(request.Email, request.Code);
        if (userSendCode is null)
        {
            await authVerificationPolicyService.IncrementVerificationAttemptsAsync(request.Email);
            return Result<string>.Failure("Код неправильный!");
        }
        #endregion

        #region CHECK SECRET
        if (request.Secret != _adminSettings.Secret)
            return Result<string>.Failure("Ошибка входа, неверный секрет!");
        #endregion

        #region CHECK USER

        var userExisting = await userRepository.GetByEmailAsync(userSendCode.Email, cancellationToken);
        if (userExisting is null)
        {
            #region VALUEOBJECTS/ONE -> DOMAIN
            Role role;

            bool exists = await adminService.ExistsRoleAdmin(cancellationToken);
            if (exists)
                return Result<string>.Failure("Ограничение: В бд не может быть больше 1 админа!");

            role = Role.Admin;

            var emailResult = Email.Create(userSendCode.Email);
            if (!emailResult.IsSuccess)
                return Result<string>.Failure(emailResult.Error!);

            var email = emailResult.Value!;
            #endregion

            var userResult = Domain.Model.Entyties.User.Create(
                email,
                role);
            if (!userResult.IsSuccess)
                return Result<string>.Failure(userResult.Error!);

            var user = userResult.Value!;

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            userExisting = user;
        }

        else
        {
            if (userExisting.Role != Role.Admin)
                return Result<string>.Failure("Ошибка входа, Неверные данные");
        }
        #endregion

        #region JWT
        var jwt = tokenService.GenerationToken(userExisting!);
        if (jwt is null)
            return Result<string>.Failure($"Ошибка генерации токена");
        #endregion

        #region DELETE KEYS
        await verificationCodeService.DeleteCodeAsync(request.Email);
        await authVerificationPolicyService.ResetVerificationAttemptsAsync(request.Email);
        #endregion

        return Result<string>.Success(jwt);
    }
}
