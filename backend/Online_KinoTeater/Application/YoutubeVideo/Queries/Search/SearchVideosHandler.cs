using Application.Common.Seacrh;
using Application.Interfaces.VideoRead;
using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Queries.Search;

public class SearchVideosHandler(
    IVideoReadService videoReadService) : IRequestHandler<SearchVideosQuery, Result<PagedResult<YoutubeVideosPreviewDto>>>
{
    public async Task<Result<PagedResult<YoutubeVideosPreviewDto>>> Handle(SearchVideosQuery request, CancellationToken cancellationToken)
    {
        var queryNormalized = SearchNormaliser.Normalize(request.Query);
        if (string.IsNullOrEmpty(queryNormalized) || queryNormalized.Length < 2)
            return Result<PagedResult<YoutubeVideosPreviewDto>>.Success(
                new PagedResult<YoutubeVideosPreviewDto> { Data = [] });

        if (queryNormalized.Length >= 25)
            return Result<PagedResult<YoutubeVideosPreviewDto>>.Failure("Поиск должен быть небольше 25 символов");

        var searched = await videoReadService.SearchPagedAsync(
            queryNormalized,
            request.Page,
            request.PageSize,
            cancellationToken);
        if (!searched.IsSuccess)
            return Result<PagedResult<YoutubeVideosPreviewDto>>.Failure(searched.Error!);

        return Result<PagedResult<YoutubeVideosPreviewDto>>.Success(searched.Value!);
    }
}
