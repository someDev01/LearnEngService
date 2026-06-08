using Application.Note.Commands.CreateNote;
using FluentValidation;

namespace Application.Validators.CreateNote;

public class CreateNoteWithContextCommandValidator: AbstractValidator<CreateNoteWithContextCommand>
{
    public CreateNoteWithContextCommandValidator()
    {
        RuleFor(p => p.Word)
            .NotNull().WithMessage("Слово не указано")
            .NotEmpty().WithMessage("Слово не может быть пустым")
            .MaximumLength(20).WithMessage("Максимальная длина слова 20 символов")
            .MinimumLength(2).WithMessage("Минимальная длина слова 2 символа");

        RuleFor(p => p.Context)
            .NotNull().WithMessage("Контекст не указан")
            .NotEmpty().WithMessage("Контекст не может быть пустым")
            .MaximumLength(100).WithMessage("Максимальная длина контекста 100 символов")
            .MinimumLength(6).WithMessage("Минимальная длина контекста 6 символа");
    }
}
