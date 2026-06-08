using Application.Auth.Commands.Send;
using Application.Common.Email;
using FluentValidation;

namespace Application.Validators.SendVerify;

public class SendVerificationsCodeCommandValidator: AbstractValidator<SendVerificationCodeCommand>
{
    public SendVerificationsCodeCommandValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Заполните email");

        When(e => !string.IsNullOrWhiteSpace(e.Email), () =>
        {
            RuleFor(p => p.Email)
                .Must(e => e.IsValidEmailAndGmail()).WithMessage("Некорректный формат почты. Почта содержит '@' и оканчивается на .ru или .com");
        });

        /*RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Заполните имя")
            .MinimumLength(5).WithMessage("Имя должно быть не меньше 5 символов")
            .MaximumLength(50).WithMessage("Имя не может превышать 50 символов");

        When(e => !string.IsNullOrWhiteSpace(e.Name), () =>
        {
            RuleFor(p => p.Name)
                .Must(n => n.IsValidName()).WithMessage("Имя должно содержать буквы и не может начинаться с чисел и други сивовлов");
        });*/
    }
}