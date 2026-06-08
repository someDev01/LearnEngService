using Application.Auth.Commands.VerifyAdmin;
using Application.Common.Email;
using FluentValidation;

namespace Application.Validators.VerifyCodeAdmin;

public class VerificationCodeAdminCommandValidator: AbstractValidator<VerificationCodeAndSecretAdminCommand>
{
    public VerificationCodeAdminCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Заполните email");

        When(e => !string.IsNullOrWhiteSpace(e.Email), () =>
        {
            RuleFor(p => p.Email)
                .Must(e => e.IsValidEmailAndGmail()).WithMessage("email должен содержать @mail и .ru");
        });

        RuleFor(p => p.Code)
            .NotEmpty().WithMessage("Введите код")
            .Length(5).WithMessage("Код должен быть 5-и значным");
    }
}
