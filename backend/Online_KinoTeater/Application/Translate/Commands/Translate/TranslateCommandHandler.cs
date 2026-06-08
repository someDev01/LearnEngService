using Application.Interfaces.Translate;
using Domain.Model.Common;
using FluentValidation;
using MediatR;

namespace Application.Translate.Commands.Translate;

public class TranslateCommandHandler(
    ITranslateService translateService,
    IValidator<TranslateWordCommand> validator) : IRequestHandler<TranslateWordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TranslateWordCommand request, CancellationToken cancellationToken)
    {
        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result<string>.Failure(errors);
        }
        #endregion

        var translationResult = await translateService.TranslateAsync(request.Word, cancellationToken);
        if (!translationResult.IsSuccess)
            return Result<string>.Failure(translationResult.Value!);

        return Result<string>.Success(translationResult.Value!);
    }
}
