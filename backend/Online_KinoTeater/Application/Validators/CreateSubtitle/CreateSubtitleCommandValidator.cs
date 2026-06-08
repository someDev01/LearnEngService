using Application.Subtitle.Commands.CreateSubtitle;
using Domain.Model.ValueObjects;
using FluentValidation;

namespace Application.Validators.CreateSubtitle;

public class CreateSubtitleCommandValidator: AbstractValidator<CreateSubtitleCommand>
{
    public CreateSubtitleCommandValidator()
    {
        RuleFor(r => r.VideoId)
            .NotEmpty().WithMessage("Заполните Id контента для создания субтитра");

        RuleFor(r => r.Format)
            .NotEmpty()
            .Must(f => Enum.TryParse<SubtitleFormat>(f, true, out _))
            .WithMessage("Некорректный формат субтитра");
    }
}
