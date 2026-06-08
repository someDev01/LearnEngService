using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Domain.Model.Common;

namespace Application.Interfaces.VideoRead;

public interface IVideoReadService
{
    Task<PagedResult<YoutubeVideosPreviewDto>> GetPagedAsync(
        int? page,
        int? pageSize,
        CancellationToken cancellationToken);

    Task<Result<PagedResult<YoutubeVideosPreviewDto>>> SearchPagedAsync(
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
