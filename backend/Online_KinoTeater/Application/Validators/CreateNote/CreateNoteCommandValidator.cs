using Application.Note.Commands.CreateNote;
using FluentValidation;

namespace Application.Validators.CreateNote;

public class CreateNoteCommandValidator: AbstractValidator<CreateNoteCommand>
{
    public CreateNoteCommandValidator()
    {
        RuleFor(p => p.EnglishName)
            .NotNull().WithMessage("Слово не указано")
            .NotEmpty().WithMessage("Слово не может быть пустым")
            .MaximumLength(25).WithMessage("Максимальная длина слова 25 символов")
            .MinimumLength(2).WithMessage("Минимальная длина слова 2 символа");

        RuleFor(p => p.Examples)
            .Must(ex => ex?.Count <= 3)
            .WithMessage("Максимальное кол-во примеров которое можно указать - 3");
    }
}
