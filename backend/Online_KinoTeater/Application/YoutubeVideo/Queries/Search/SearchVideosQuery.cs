using Application.SharedDtos;
using Application.YoutubeVideo.Dtos;
using Domain.Model.Common;
using MediatR;

namespace Application.YoutubeVideo.Queries.Search;

public record SearchVideosQuery(
    string Query,
    int Page,
    int PageSize): IRequest<Result<PagedResult<YoutubeVideosPreviewDto>>>;
