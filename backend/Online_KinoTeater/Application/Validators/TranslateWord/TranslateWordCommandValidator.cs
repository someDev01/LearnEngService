using Application.Translate.Commands.Translate;
using FluentValidation;

namespace Application.Validators.TranslateWord;

public class TranslateWordCommandValidator: AbstractValidator<TranslateWordCommand>
{
    public TranslateWordCommandValidator()
    {
        RuleFor(p => p.Word)
            .NotNull().WithMessage("Слово не указано")
            .NotEmpty().WithMessage("Слово не может быть пустым");
    }
}
