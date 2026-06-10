using Application.Note.Commands.UpdateNote;
using FluentValidation;

namespace Application.Validators.UpdateNote;

public class UpdateNoteCommandValidator: AbstractValidator<UpdateNoteCommand>
{
    public UpdateNoteCommandValidator()
    {
        RuleFor(p => p.Word)
            .MaximumLength(25).WithMessage("Максимальная длина слова 25 символов")
            .MinimumLength(2).WithMessage("Минимальная длина слова 2 символа");
        
        RuleFor(p => p.Translations)
            .Must(t => t?.Count <= 3)
            .WithMessage("Максимальное кол-во переводов которое можно указать - 3");
        
        RuleFor(p => p.Examples)
            .Must(ex => ex?.Count <= 3)
            .WithMessage("Максимальное кол-во примеров которое можно указать - 3");
    }
}