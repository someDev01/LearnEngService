using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Subtitle;
using FluentValidation;
using MediatR;

namespace Application.Subtitle.Commands.UpdateSubtitle;

public class UpdateSubtitleCommandHandler(
    ISubtitleRepository subtitleRepository,
    IValidator<UpdateSubtitleCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateSubtitleCommand, Result>
{
    public async Task<Result> Handle(UpdateSubtitleCommand request, CancellationToken cancellationToken)
    {
        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result<Guid>.Failure(errors);
        }
        #endregion

        var subtitle = await subtitleRepository.GetByIdAsync(request.SubtitleId, cancellationToken);

        if (subtitle is null)
            return Result.Failure($"Субтитр под таким Id {request.SubtitleId} не найден!");

        #region PROPERTIES
        if (!string.IsNullOrWhiteSpace(request.File))
        {
            var result = subtitle.UpdateFile(request.File);
            if (!result.IsSuccess) return Result.Failure(result.Error!);
        }

        if (!string.IsNullOrWhiteSpace(request.Format))
        {
            Enum.TryParse<SubtitleFormat>(request.Format, true, out var format);
            var result = subtitle.UpdateFormat(format);
            if (!result.IsSuccess) return Result.Failure(result.Error!);
        }
        #endregion

        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success();
    }
}
