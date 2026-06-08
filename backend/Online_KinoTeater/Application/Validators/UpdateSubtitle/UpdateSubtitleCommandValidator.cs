using Application.Subtitle.Commands.UpdateSubtitle;
using Domain.Model.ValueObjects;
using FluentValidation;

namespace Application.Validators.UpdateSubtitle;

public class UpdateSubtitleCommandValidator: AbstractValidator<UpdateSubtitleCommand>
{
    public UpdateSubtitleCommandValidator()
    {
        RuleFor(r => r.Format)
            .NotEmpty()
            .Must(f => Enum.TryParse<SubtitleFormat>(f, true, out _))
            .WithMessage("Некорректный формат субтитра");
    }
}
