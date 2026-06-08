using Application.Interfaces.VideoRead;
using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Application.YoutubeVideo.Queries.GetAllVideos;
using Domain.Model.Common;
using MediatR;


namespace Application.YoutubeVideo.Queries.GetPagedVideos;

public class GetYoutubeVideosHandler(
    IVideoReadService videoReadService) :
    IRequestHandler<GetYoutubeVideosQuery, Result<PagedResult<YoutubeVideosPreviewDto>>>
{
    public async Task<Result<PagedResult<YoutubeVideosPreviewDto>>> Handle(
        GetYoutubeVideosQuery request, CancellationToken cancellationToken)
    {
        var pagedNotes = await videoReadService.GetPagedAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        return Result<PagedResult<YoutubeVideosPreviewDto>>.Success(pagedNotes);
    }
}
