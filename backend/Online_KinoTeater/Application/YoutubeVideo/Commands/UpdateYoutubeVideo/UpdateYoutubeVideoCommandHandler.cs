using Application.Interfaces.UnitOfWork;
using Application.Interfaces.VideoCache;
using Domain.Model.Common;
using Domain.Repositories.Video;
using MediatR;

namespace Application.YoutubeVideo.Commands.UpdateYoutubeVideo;

public class UpdateYoutubeVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoCacheService videoCacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateYoutubeVideoCommand, Result>
{
    public async Task<Result> Handle(UpdateYoutubeVideoCommand request, CancellationToken cancellationToken)
    {
        var youtubeVideo = await videoRepository.GetByIdAsync(request.Id, cancellationToken);
        if (youtubeVideo is null)
            return Result.Failure($"Ютуб видео с таким id не найден");

        #region PROPERTIES
        if (request.Title is not null)
        {
            var result = youtubeVideo.UpdateTitle(request.Title);
            if (!result.IsSuccess) return Result.Failure(result.Error!);
        }
        #endregion

        #region DTO -> VALUEOBJECTS/ONE
        if (request.LexicalComplexity is not null)
        {
            var result = youtubeVideo.UpdateLexicalComplexity(request.LexicalComplexity);
            if (!result.IsSuccess) return Result.Failure(result.Error!);
        }
        #endregion

        await unitOfWork.CommitAsync(cancellationToken);

        await videoCacheService.InvalidatePagedVideoAsync(cancellationToken);

        return Result.Success();
    }
}
