using Application.Interfaces.Storage;
using Application.Interfaces.UnitOfWork;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Subtitle;
using Domain.Repositories.Video;
using FluentValidation;
using MediatR;

namespace Application.Subtitle.Commands.CreateSubtitle;

public class CreateSubtitleCommandHandler(
    ISubtitleRepository subtitleRepository,
    IVideoRepository videoRepository,
    IFileStorageService fileStorageService,
    IValidator<CreateSubtitleCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateSubtitleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSubtitleCommand request, CancellationToken cancellationToken)
    {
        #region VALIDATION
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result<Guid>.Failure(errors);
        }

        #endregion

        #region CHECK EXISTS
        var youtubeVideo = await videoRepository.GetByIdAsync(request.VideoId, cancellationToken);
        if(youtubeVideo is null)
            return Result<Guid>.Failure("Такое видео не найдено");

        var exists = await subtitleRepository.ExistsAsync(
            request.VideoId,
            request.Language,
            cancellationToken);

        if (exists)
            return Result<Guid>.Failure($"Субтитр : " +
                $" '{request.Language}' уже есть ");

        Enum.TryParse<SubtitleFormat>(request.Format, true, out var format);
        #endregion

        #region UPLOAD S3
        var key = $"subtitles/{request.Key}";

        var uploadResult = await fileStorageService.UploadAsync(
            request.FileStream,
            key,
            request.ContentType,
            cancellationToken);
        if (!uploadResult.IsSuccess)
            return Result<Guid>.Failure(uploadResult.Error!);
        #endregion

        #region VALUEOBJECTS/ONE -> DOMAIN
        var languageResult = Language.Create(request.Language);
        if (!languageResult.IsSuccess)
            return Result<Guid>.Failure(languageResult.Error!);
        #endregion

        #region CREATE
        var subtitleResult = Domain.Model.Entyties.Subtitle.Create(
            request.VideoId,
            Language.Create(request.Language).Value!,
            uploadResult.Value!,
            format);

        if (!subtitleResult.IsSuccess)
            return Result<Guid>.Failure(subtitleResult.Error!);

        var subtitle = subtitleResult.Value!;

        await subtitleRepository.AddAsync(subtitle, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        #endregion

        return Result<Guid>.Success(subtitle.Id);
    }
}
