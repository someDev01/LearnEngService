using Application.Common.YoutubeId;
using Application.Interfaces.Clients.Youtube;
using Application.Interfaces.UnitOfWork;
using Application.Interfaces.VideoCache;
using Application.YoutubeVideo.Commands.AddYoutubeVideo;
using Domain.Model.Common;
using Domain.Model.ValueObjects;
using Domain.Repositories.Video;
using MediatR;

namespace Application.YoutubeVideo.Commands.CreateYoutubeVideo;

public class CreateYoutubeVideoCommandHandler(
    IVideoRepository videoRepository,
    IYoutubeClient youtubeService,
    IVideoCacheService videoCacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateYoutubeVideoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateYoutubeVideoCommand request, CancellationToken cancellationToken)
    {
        #region CHECK EXISTS
        var youtubeIdResult = YoutubeIdExtension.GetYoutubeId(request.YoutubeVideoUrl);
        if (!youtubeIdResult.IsSuccess)
            return Result<Guid>.Failure(youtubeIdResult.Error!);

        var youtubeId = youtubeIdResult.Value;

        var exists = await videoRepository.ExistsAsync(youtubeId!, cancellationToken);
        if (exists)
            return Result<Guid>.Failure("Такое видое с YoutubeId есть  у данного контента");
        #endregion

        #region YOUTUBEINFO
        var youtubeVideoInfo = await youtubeService.GetVideoAsync(youtubeId!);
        if (youtubeVideoInfo is null)
            return Result<Guid>.Failure("Не удалось получить длительность видео");
        #endregion

        #region PROPERTIES
        var youtubeTitleVideo = youtubeVideoInfo.Title;
        #endregion

        #region VALUEOBJECTS/ONE -> DOMAIN
        var durationResult = Duration.Create(
            youtubeVideoInfo.Hours,
            youtubeVideoInfo.Minutes,
            youtubeVideoInfo.Seconds);
        if (!durationResult.IsSuccess)
            return Result<Guid>.Failure(durationResult.Error!);

        var lexicalComplexityResult = LexicalComplexity.Create(request.LexicalComplexity);
        if (!lexicalComplexityResult.IsSuccess)
            return Result<Guid>.Failure(lexicalComplexityResult.Error!);
        #endregion

        #region CREATE
        var youtubeVideoResult = Domain.Model.Entyties.YoutubeVideo.Create(
            youtubeId!,
            youtubeTitleVideo,
            durationResult.Value!,
            lexicalComplexityResult.Value!);
        if (!youtubeVideoResult.IsSuccess)
            return Result<Guid>.Failure(youtubeVideoResult.Error!);

        var youtubeVideo = youtubeVideoResult.Value!;

        await videoRepository.AddAsync(youtubeVideo, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        #endregion

        #region DELETE KEY
        await videoCacheService.InvalidatePagedVideoAsync(cancellationToken);
        #endregion

        return Result<Guid>.Success(youtubeVideo.Id);
    }
}
