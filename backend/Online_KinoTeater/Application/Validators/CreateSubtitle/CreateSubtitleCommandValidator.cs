using Application.Subtitle.Commands.CreateSubtitle;
using Domain.Model.Enum;
using Domain.Model.ValueObjects;
using FluentValidation;

namespace Application.Validators.CreateSubtitle;

public class CreateSubtitleCommandValidator: AbstractValidator<CreateSubtitleCommand>
{
    public CreateSubtitleCommandValidator()
    {
        RuleFor(r => r.VideoId)
            .NotEmpty().WithMessage("Заполните Id контента для создания субтитра");

        RuleFor(r => r.LanguageCode)
            .NotEmpty().WithMessage("Языковая дорожка не может быть пустой");

        RuleFor(r => r.LanguageCode)
            .Must(l => !int.TryParse(l, out _) &&
                Enum.TryParse<LanguageCode>(l, ignoreCase: true, out var languageResult) && 
                Enum.IsDefined(languageResult))
            .WithMessage("Некорректное значение языковой дорожки")
            .When(r => !string.IsNullOrEmpty(r.LanguageCode)); 

        RuleFor(r => r.Format)
            .NotEmpty()
            .Must(f => Enum.TryParse<SubtitleFormat>(f, true, out _))
            .WithMessage("Некорректный формат субтитра");
    }
}
