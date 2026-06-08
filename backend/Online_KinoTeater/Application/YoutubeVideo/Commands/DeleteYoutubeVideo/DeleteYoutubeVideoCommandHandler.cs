using Application.Interfaces.UnitOfWork;
using Application.Interfaces.VideoCache;
using Domain.Model.Common;
using Domain.Repositories.Video;
using MediatR;

namespace Application.YoutubeVideo.Commands.DeleteYoutubeVideo;

public class DeleteYoutubeVideoCommandHandler(
    IVideoRepository videoRepository,
    IVideoCacheService videoCacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteYoutubeVideoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DeleteYoutubeVideoCommand request, CancellationToken cancellationToken)
    {
        var youtubeVideo = await videoRepository.GetByIdAsync(request.YoutubeVideoId, cancellationToken);
        if (youtubeVideo is null)
            return Result<Guid>.Failure("Такого видео нет");

        await videoRepository.DeleteAsync(youtubeVideo, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        await videoCacheService.InvalidatePagedVideoAsync(cancellationToken);

        return Result<Guid>.Success(youtubeVideo.Id);
    }
}
